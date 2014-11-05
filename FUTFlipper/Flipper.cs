using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using UltimateTeam.Toolkit;
using UltimateTeam.Toolkit.Models;
using UltimateTeam.Toolkit.Extensions;
using UltimateTeam.Toolkit.Parameters;
using UltimateTeam.Toolkit.Exceptions;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;
using System.IO;
using FlipperUtil;

namespace FUTFlipper
{
    public class Flipper
    {
        private Dictionary<string, UserDetails> loginDetails;

        private ILog Log;
        private IFutClient futClient;
        private uint Credits { get; set; }
        private int WatchListCount { get; set; }
        private int WatchListWonCount { get; set; }
        private int TradePileCount { get; set; }
        private const int MaxTradePileSize = 40;
        private const int MaxWatchListSize = 140;
        List<AuctionInfo> SearchResults;
        private Random random;
        public int LastSearchCount { get; set; }
        public int LastSearchFirstExpiry { get; set; }
        public int LastSearchLastExpiry { get; set; }
        private DateTime NextWatchListSearchTime;
        private DateTime NextTradePileSearchTime;
        private DateTime NextTakeABreathTimeTime;
        private bool searchWatchListOverride;
        public string Account { get; set; }
        public bool SessionExpired { get; set; }
        public bool SlowDay { get; set; }
        public bool CouldNotLogin { get; set; }
        public bool LoggedIn { get; set; }
        public List<long> UnsellableItemsCaptured { get; set; }

        private SpreadsheetsService gDocsService;
        private ListFeed listFeed;
        private int slowDayConsectutiveCount = 0;
        private int normalDayConsectutiveCount = 0;
        private DateTime NextLogToGoogleTime;

        private DateTime ExceptionEventWindowStart;
        private int ExceptionsEventDuringWindow;
        private const int ExceptionEventWindowLengthMinutes = 2;
        private const int ExceptionEventThreshold = 20;

        public Flipper()
        {
            LoadUserDetails();

            WatchListCount = WatchListWonCount = TradePileCount = -1;
            SearchResults = new List<AuctionInfo>();
            random = new Random();
            NextWatchListSearchTime = NextTradePileSearchTime = NextLogToGoogleTime = DateTime.MinValue;
            NextTakeABreathTimeTime = DateTime.Now.AddMinutes(4);
            searchWatchListOverride = false;
            SessionExpired = false;
            Page = 1;
            SlowDay = false;
            LoggedIn = false;
            UnsellableItemsCaptured = new List<long>();

            gDocsService = new SpreadsheetsService("Ken Nguyen gDoc Service");
            gDocsService.setUserCredentials("nguyen.kirk@gmail.com", "ilovedaddy");
        }

        public void ChangeAccount(string name)
        {
            Account = name;
            log4net.GlobalContext.Properties["LogName"] = Account;
            Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log4net.Config.XmlConfigurator.Configure();
        }

        public async Task BuyBuyBuy(uint page)
        {
            await Login();
            Page = page;
            while (true)
            {
                if (CouldNotLogin)
                {
                    System.Threading.Thread.Sleep(5 * 60000);
                    await Login();
                }
                else
                {
                    await PlayerForQuickSellHoverSearch();

                    if (TooManyExceptions())
                    {
                        Log.Error("Too Many Exceptions: {0} in {1} seconds. Have a break for 30 mins".Args(ExceptionsEventDuringWindow, (DateTime.UtcNow - ExceptionEventWindowStart).TotalSeconds));
                        System.Threading.Thread.Sleep(30 * 60000);
                        await Login();
                        SessionExpired = false;
                    }

                    System.Threading.Thread.Sleep((new Random()).Next(55000, 65000));

                    if (SessionExpired)
                    {
                        await Login();
                        SessionExpired = false;
                    }
                }
            }

        }

        private int failedLoginCount = 0;
        public async Task Login()
        {
            try
            {
                CouldNotLogin = false;
                Log.Info("Logging in with {0}...".Args(loginDetails[Account].LoginDetails.Username));
                futClient = new FutClient();
                var loginResponse = await futClient.LoginAsync(loginDetails[Account].LoginDetails);
                SetupGoogle();

                if (loginResponse != null)
                {
                    LoggedIn = true;
                    Log.Info("Logged in with {0}...".Args(loginResponse.UserAccounts.UserAccountInfo.Personas.First().PersonaName));
                }
                else
                {
                    CouldNotLogin = true;
                    Log.Warn("Could not login");
                    RecordExceptionEvent();
                }
            }
            catch (Exception ex)
            {
                RecordExceptionEvent();
                Log.Warn("Could not login: {0}".Args(ex.Message));
                CouldNotLogin = true;
            }


        }

        public async void LoginAgain(Exception ex, Func<Task> returnFunc)
        {
            if (failedLoginCount < 10)
            {
                int backoffDelay = failedLoginCount++ * 10;

                Log.Warn("Logging in again. Backoff {0}. {1}".Args(backoffDelay, ex == null ? "" : ex.Message));

                System.Threading.Thread.Sleep(backoffDelay * 1000);

                await Login();
                Log.Info("Going back to original function");
                await returnFunc();
            }
            else
            {
                Log.Warn("No more logging in. Failed login count has reached maximum");
            }
        }

        public uint Page { get; set; }
        private uint pageInc = 32;
        private bool goingUp = true;
        public async Task PlayerForQuickSellHoverSearch()
        {
            if ((searchWatchListOverride && NextWatchListSearchTime == DateTime.MaxValue) || DateTime.Now > NextWatchListSearchTime)
            {
                await WatchList();
            }
            else
            {
                Log.Info("Waiting til {0} to look at watch list again".Args(NextWatchListSearchTime));
            }

            if (DateTime.Now > NextTradePileSearchTime)
            {
                await TradePile();
            }

            while (Credits > 250 && WatchListCount < MaxWatchListSize)
            {
                bool searchSuccessful = await PlayerForQuickSellSearch(Page);
                HumanDelay();
                if (searchSuccessful)
                {
                    if (!(LastSearchFirstExpiry < 3600 && LastSearchLastExpiry > 3600)) // we're not on the page that is on the 60 min mark
                    {
                        if ((/*LastSearchCount < pageSize || */LastSearchLastExpiry > 3600) && Page > 1)
                        {
                            if (goingUp)
                            {
                                if (pageInc > 1) pageInc /= 2;
                                goingUp = false;
                            }
                            if (Page > pageInc)
                            {
                                Page -= pageInc;
                            }
                            else
                            {
                                Page = 1;
                            }
                        }
                        else
                        {
                            if (!goingUp)
                            {
                                if (pageInc > 1) pageInc /= 2;
                                goingUp = true;
                            }

                            Page += pageInc;
                        }
                    }
                }

                if ((searchWatchListOverride && NextWatchListSearchTime == DateTime.MaxValue) || DateTime.Now > NextWatchListSearchTime)
                {
                    await WatchList();
                    await UnnassignedPile();
                }

                
                if (!SlowDay && Page < 150)
                {
                    slowDayConsectutiveCount++;
                    if (slowDayConsectutiveCount >= 10)
                    {
                        SlowDay = true;
                    }
                }
                else
                {
                    slowDayConsectutiveCount = 0;
                }
                if (SlowDay && Page > 180)
                {
                    normalDayConsectutiveCount++;
                    if (normalDayConsectutiveCount >= 10)
                    {
                        SlowDay = false;
                    }
                }
                else
                {
                    normalDayConsectutiveCount = 0;
                }
                //Log.Info("{2}, slow: {0}, normal: {1}".Args(slowDayConsectutiveCount, normalDayConsectutiveCount, SlowDay));
                
                if (DateTime.Now > NextTakeABreathTimeTime)
                {
                    Log.Info("Take a breath");
                    HumanDelay(20); // have a break from searching
                    NextTakeABreathTimeTime = DateTime.Now.AddMinutes(2);
                }

                if (DateTime.Now > NextTradePileSearchTime)
                {
                    await TradePile();
                }

                if (SessionExpired)
                {
                    break;
                }

                if (TooManyExceptions())
                {
                    return;
                }
            }

            Log.Info("Finishing PlayerForQuickSellHoverSearch... Time for long sleep");
        }

        private bool StillFarOff60Mins(int time)
        {
            return Math.Abs(time - 3600) > 120;
        }

        public async void SearchToTransferMoney(int buyAmount, string buyType)
        {

            SearchParameters searchParameters;

            switch (buyType)
            {
                case "Staff":
                    searchParameters = new StaffSearchParameters
                    {
                        Page = 1,
                        Level = Level.Gold,
                        MaxBuy = (uint)buyAmount,
                        MinBuy = (uint)(buyAmount - CalculateBidIncrement(buyAmount)),
                        PageSize = (byte)(pageSize - 1)
                    };
                    break;
                case "ClubInfo":
                    searchParameters = new ClubInfoSearchParameters
                    {
                        ClubInfoType = ClubInfoType.Kit,
                        Page = 1,
                        MaxBuy = (uint)buyAmount,
                        MinBuy = (uint)(buyAmount - CalculateBidIncrement(buyAmount)),
                        PageSize = (byte)(pageSize - 1)
                    };
                    break;
                default:
                    searchParameters = new PlayerSearchParameters
                    {
                        Page = 1,
                        Level = Level.Gold,
                        MaxBuy = (uint)buyAmount,
                        MinBuy = (uint)(buyAmount - CalculateBidIncrement(buyAmount)),
                        PageSize = (byte)(pageSize - 1),
                        League = League.LigaBbva,
                        Position = Position.RightBack
                    };
                    break;
            }

            AuctionResponse searchResponse = await futClient.SearchAsync(searchParameters);
            AuctionInfo item =  searchResponse.AuctionInfo.Where(a => a.SellerName == "Shin Kickers").FirstOrDefault();
            if (item == null)
            {
                Log.Info("Could not find anything to buy");
            }
            else
            {
                BuyNow(item);
                Log.Info("Buying item {0} from {1} for {2}".Args(item.ItemData.Id, item.SellerName, item.BuyNowPrice));
            }
        }

        private int CalculateBidIncrement(int bid)
        {
            if (bid < 1000)
                return 50;

            if (bid < 10000)
                return 100;

            if (bid < 50000)
                return 250;

            if (bid < 100000)
                return 500;

            return 1000;
        }

        private async void BuyNow(AuctionInfo item)
        {
            var auctionResponse = await futClient.PlaceBidAsync(item, item.BuyNowPrice);
        }

        private int pageSize = 15;
        public async Task<bool> PlayerForQuickSellSearch(uint searchPage)
        {
            var searchParameters = new PlayerSearchParameters
            {
                Page = searchPage,
                Level = Level.Gold,
                MaxBid = SlowDay ? 250u : 200u,
                PageSize = (byte)(pageSize - 1)
            };
            Log.Info("Searching page {0}".Args(searchPage));
            AuctionResponse searchResponse;
            try
            {
                searchResponse = await futClient.SearchAsync(searchParameters);
            }
            catch (ExpiredSessionException e)
            {
                RecordExceptionEvent();
                SessionExpired = true;
                return false;
            }
            catch(Exception e)
            {
                RecordExceptionEvent();
                Log.Info("There was a problem searching, try again");
                return false;
            }

            if (searchResponse == null || searchResponse.AuctionInfo == null)
            {
                Log.Info("searchResponse{0} was null, try again".Args(searchResponse == null ? "" : ".AuctionInfo"));
                HumanDelay();
                return false;
            }
            else if (searchResponse.AuctionInfo.Any())
            {
                LastSearchCount = searchResponse.AuctionInfo.Count();
                LastSearchFirstExpiry = searchResponse.AuctionInfo.First().Expires;
                LastSearchLastExpiry = searchResponse.AuctionInfo.Last().Expires;
                Credits = searchResponse.Credits;
                Log.Info("{3} Count: {2}, Expires: First {0}, Last {1}, {4}".Args(LastSearchFirstExpiry.ExpirySecondsToMin(), LastSearchLastExpiry.ExpirySecondsToMin(), LastSearchCount, Credits, SlowDay ? "Slow" : "Normal"));
                var bidAble = searchResponse.AuctionInfo
                    .Where(item => item.ItemData.RareFlag == 0 && item.ItemData.DiscardValue.HasValue && item.Expires < 4200 && BiddableBasedOnRating(item.ItemData.Rating));

                Bid(bidAble);
            }
            else
            {
                LastSearchCount = 0;
                LastSearchFirstExpiry = LastSearchLastExpiry = Int32.MaxValue;
                Log.Info("{0} Count: 0".Args(Credits));
            }

            HumanDelay();
            return true;
        }

        private bool BiddableBasedOnRating(byte rating)
        {
            if (SlowDay)
            {
                return rating < 80;
            }
            else
            {
                return rating == 75;
            }
        }

        private async void Bid(IEnumerable<AuctionInfo> auctionInfos)
        {
            foreach (var auctionInfo in auctionInfos)
            {
                uint amountIWantToBid = BidAmountJustUnder(auctionInfo.ItemData.DiscardValue.Value);
                amountIWantToBid = auctionInfo.BuyNowPrice > 0 ? Math.Min(amountIWantToBid, auctionInfo.BuyNowPrice) : amountIWantToBid;
                if (Math.Max(auctionInfo.CurrentBid, auctionInfo.StartingBid) < amountIWantToBid && Credits > amountIWantToBid)
                {
                    Log.Info("Bidding on " + auctionInfo.ItemData.Rating + "|" + auctionInfo.Expires.ExpirySecondsToMin() + "|" + auctionInfo.TradeId + "|" + Math.Max(auctionInfo.CurrentBid, auctionInfo.StartingBid) + "|" + amountIWantToBid + "|" + auctionInfo.ItemData.DiscardValue.ToString());
                    try
                    {
                        var tradeStatusResult = await futClient.GetTradeStatusAsync(new List<long>() { auctionInfo.TradeId });
                        HumanDelay();
                        var tradeStatus = tradeStatusResult.AuctionInfo.Where(t => t.TradeId == auctionInfo.TradeId);
                        if (tradeStatus.Any() && tradeStatus.First().CurrentBid < amountIWantToBid)
                        {
                            await futClient.PlaceBidAsync(auctionInfo, amountIWantToBid);
                            HumanDelay();
                            Credits -= amountIWantToBid;
                            Log.Info("    Bid complete");
                            searchWatchListOverride = true;
                        }
                        else
                        {
                            string msg = "item doesn't exist";
                            if (tradeStatus.Any()) {
                                msg = "current bid {0}".Args(tradeStatus.First().CurrentBid);
                            }
                            Log.Info("    Bid was pipped {0}".Args(msg));
                        }
                    }
                    catch (ExpiredSessionException e)
                    {
                        RecordExceptionEvent();
                        Log.Info("Session Expired");
                        SessionExpired = true;
                    }
                    catch
                    {
                        //RecordExceptionEvent();
                        Log.Info("    Bid failed");
                    }                   
                }
            }
        }

        public async Task WatchList()
        {
            searchWatchListOverride = false;
            try
            {
                //Log.Info("Slow Day: " + SlowDay);
                Log.Info("Searching watchlist...");
                var searchResponse = await futClient.GetWatchlistAsync();
                HumanDelay();

                if (searchResponse.AuctionInfo != null)
                {
                    Credits = searchResponse.Credits;
                    if (searchResponse.AuctionInfo.Any())
                    {
                        var watchingSorted = searchResponse.AuctionInfo.Where(co => !loginDetails[Account].UnsellableItems.Contains(co.ItemData.Id) && !UnsellableItemsCaptured.Contains(co.ItemData.Id)).OrderBy(ai => ai.Expires);
                        if (UnsellableItemsCaptured.Any())
                        {
                            Log.Info("Ignoring unsellable items: {0}".Args(String.Join("|", UnsellableItemsCaptured.Select(i => { return i.ToString(); }))));
                        }
                        var nonExpired = watchingSorted.Where(w => w.Expires > 0);
                        if (nonExpired.Any())
                        {
                            int nextExpiredTime = nonExpired.First().Expires;
                            foreach (AuctionInfo ai in nonExpired)
                            {
                                if (ai.Expires - 15 <= nextExpiredTime)
                                {
                                    nextExpiredTime = ai.Expires;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            NextWatchListSearchTime = DateTime.Now.AddSeconds(Math.Min(nextExpiredTime + 3, 90));
                        }
                        foreach (var auctionInfo in watchingSorted)
                        {
                            Log.Info(auctionInfo.BidState + "|" + auctionInfo.Expires.ExpirySecondsToMin() + "|" + auctionInfo.BuyNowPrice + "|" + auctionInfo.StartingBid + "|" + auctionInfo.CurrentBid + "|" + auctionInfo.ItemData.DiscardValue.ToString());
                        }

                        var won = watchingSorted.Where(co => co.TradeState == "closed" && co.BidState == "highest");
                        var lost = watchingSorted.Where(f => f.BidState == "outbid");

                        Log.Info(Credits + " Removing lost bids " + lost.Count());
                        await futClient.RemoveFromWatchlistAsync(lost);
                        Log.Info(Credits + " Lost bids removed.");
                        HumanDelay();

                        if (won.Any())
                        {
                            Log.Info(Credits + " Quick selling");
                            var newCredits = Credits;
                            foreach (var w in won.OrderBy(w => w.ItemData.Id))
                            {
                                try
                                {
                                    Log.Info("Quick selling {0}, rating {1} for {2} credits".Args(w.ItemData.Id, w.ItemData.Rating, w.ItemData.DiscardValue.ToString()));
                                    HumanDelay(2);
                                    var quickSellResponse = await futClient.QuickSellItemAsync(w.ItemData.Id);
                                    newCredits = (uint)quickSellResponse.TotalCredits;
                                }
                                catch (Exception ex)
                                {
                                    UnsellableItemsCaptured.Add(w.ItemData.Id);
                                    Log.Error("Error trying to quick sell item {0}: {1}".Args(w.ItemData.Id, ex.Message));
                                }
                            }

                            var soldFor = newCredits - Credits;
                            Credits = newCredits;
                            Log.Info(Credits + " Finished quick selling for total of {0}.".Args(soldFor));
                        }
                        WatchListCount = watchingSorted.Count() - won.Count() - lost.Count();
                        Log.Info(Credits + " Still watching {0} items.".Args(WatchListCount));
                        long totalCredits = Credits + nonExpired.Where(f => f.BidState != "outbid").Sum(f => f.CurrentBid);
                        Log.Info("Total value {0}".Args(totalCredits));
                        LogToGoogle(totalCredits);

                        if (nonExpired.Count() + won.Count() + lost.Count() == 0)
                        {
                            NextWatchListSearchTime = DateTime.MaxValue;
                            Log.Info(Credits + " Nothing found in the end.");
                        }

                        HumanDelay();
                    }
                    else
                    {
                        NextWatchListSearchTime = DateTime.MaxValue;
                        Log.Info(Credits + " ...Nothing found.");
                        LogToGoogle(Credits);
                    }

                    failedLoginCount = 0;
                }
                else
                {
                    Log.Warn("Oops No response when Searching for WatchList.");
                    //LoginAgain(null, WatchList);
                }
            }
            catch (ExpiredSessionException e)
            {
                RecordExceptionEvent();
                Log.Info("Session Expired");
                SessionExpired = true;
            }
            catch (Exception ex)
            {
                RecordExceptionEvent();
                Log.Warn("Oops Exception Searching for WatchList.");
                //LoginAgain(ex, WatchList);
            }
        }

        public async Task TradePile()
        {
            Log.Info("Searching trade pile...");
            try
            {
                var tradePileResponse = await futClient.GetTradePileAsync();
                if (tradePileResponse.AuctionInfo != null)
                {
                    Credits = tradePileResponse.Credits;
                    if (tradePileResponse.AuctionInfo.Any())
                    {
                        var tradePileOrdered = tradePileResponse.AuctionInfo.OrderBy(a => a.Expires);
                        var nonExpired = tradePileOrdered.Where(a => a.Expires > 0);
                        if (nonExpired.Any())
                        {
                            NextTradePileSearchTime = DateTime.Now.AddSeconds(nonExpired.First().Expires + 3);
                        }
                        else
                        {
                            NextTradePileSearchTime = DateTime.Now.AddHours(1);
                        }
                        foreach (var auctionInfo in tradePileOrdered)
                        {
                            Log.Info(auctionInfo.TradeState + "|" + auctionInfo.Expires.ExpirySecondsToMin() + "|" + auctionInfo.BuyNowPrice + "|" + auctionInfo.StartingBid + "|" + auctionInfo.CurrentBid + "|" + auctionInfo.ItemData.LastSalePrice);
                        }

                        //var sold = tradePileResponse.AuctionInfo.Where(a => a.TradeState == "closed" && a.IsContractCard());
                        var expired = tradePileResponse.AuctionInfo.Where(a => a.TradeState == "expired");

                        /*IEnumerable<Task> removeSoldTasks = sold.Select(s =>
                        {
                            Log.Info("Remove sold item " + s.TradeId);
                            return tradePileRequest.RemoveFromTradePileAsync(s.TradeId);
                        });*/
                        IEnumerable<Task<ListAuctionResponse>> listAuctionTasks = expired.Select(e =>
                        {
                            Log.Info("ReAuctioning {0} for starting {1}, buynow {2}".Args(e.TradeId, e.StartingBid, e.BuyNowPrice));
                            HumanDelay();
                            return futClient.ListAuctionAsync(new AuctionDetails(e.ItemData.Id, AuctionDuration.OneHour, e.StartingBid, e.BuyNowPrice)); // use old BuyNow and StartingBid
                        });

                        TradePileCount = tradePileResponse.AuctionInfo.Count();
                        Log.Info(Credits + " TradePileCount " + TradePileCount);
                        await Task.WhenAll(listAuctionTasks);
                        Log.Info(Credits + " ReAuctioning all expired items");

                    }
                    else
                    {
                        NextTradePileSearchTime = DateTime.Now.AddHours(1);
                        Log.Info(Credits + " ...Nothing found.");
                    }

                    failedLoginCount = 0;
                }
                else
                {
                    string msg = "Oops No response when Searching for TradePile.";
                    Log.Warn(msg);
                }
            }
            catch (ExpiredSessionException e)
            {
                RecordExceptionEvent();
                Log.Info("Session Expired");
                SessionExpired = true;
            }
            catch (Exception ex)
            {
                RecordExceptionEvent();
                Log.Warn("Oops No response when Searching for TradePile.");
            }
            HumanDelay();
        }

        public async Task UnnassignedPile()
        {
            Log.Info("Searching unassigned pile...");
            try
            {
                var purchasedItemsResponse = await futClient.GetPurchasedItemsAsync();
                if (purchasedItemsResponse.ItemData.Any())
                {
                    Log.Info(Credits + " Quick selling unassigned pile");
                    IEnumerable<Task<QuickSellResponse>> quickSellTasks = purchasedItemsResponse.ItemData.Select(id =>
                    {
                        Log.Info("Quick selling {0}, rating {1} for {2} credits".Args(id.Id, id.Rating, id.DiscardValue.ToString()));
                        HumanDelay();
                        return futClient.QuickSellItemAsync(id.Id);
                    });

                    var quickSellResponses = await Task.WhenAll(quickSellTasks);
                    var newCredits = (uint)quickSellResponses.Max(r => r.TotalCredits);
                    var soldFor = newCredits - Credits;
                    Credits = newCredits;
                    Log.Info(Credits + " Finished quick selling for total of {0}.".Args(soldFor));

                }
                else
                {
                    string msg = "Unnassigned Pile empty.";
                    Log.Warn(msg);
                }
            }
            catch (ExpiredSessionException e)
            {
                RecordExceptionEvent();
                Log.Info("Session Expired");
                SessionExpired = true;
            }
            catch (Exception ex)
            {
                RecordExceptionEvent();
                Log.Warn("Oops No response when Searching for UnnassignedPile.");
            }

            HumanDelay();
        }

        private void HumanDelay(int weight = 1)
        {
            System.Threading.Thread.Sleep(random.Next(2000, 2500) * weight);
        }

        private uint BidAmountJustUnder(uint value)
        {
            return ((value / 50) - ((uint)(value % 50 == 0 ? 1 : 0))) * 50;
            //return ((value / 50) - 1) * 50;
        }

        private void SetupGoogle()
        {
            try
            {
                SpreadsheetQuery query = new SpreadsheetQuery();
                SpreadsheetFeed feed = gDocsService.Query(query);
                var sheet = (SpreadsheetEntry)(from x in feed.Entries where x.Title.Text.Contains("FUT coins") select x).First();

                WorksheetFeed wsFeed = sheet.Worksheets;
                WorksheetEntry worksheet = (WorksheetEntry)wsFeed.Entries.Where(e => e.Title.Text == Account).First();

                AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

                // Fetch the list feed of the worksheet.
                ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
                listFeed = gDocsService.Query(listQuery);
            }
            catch (Exception ex)
            {
                Log.Warn("Couldn't set up Google: " + ex.Message); 
            }
        }

        private void LogToGoogle(long totalCredits)
        {
            if (DateTime.Now >= NextLogToGoogleTime)
            {
                Task.Factory.StartNew(() =>
                {
                    if (listFeed == null)
                    {
                        Log.Warn("Need to Setup Google");
                        SetupGoogle();
                    }

                    try
                    {
                        ListEntry row = new ListEntry();
                        row.Elements.Add(new ListEntry.Custom() { LocalName = "firstname", Value = DateTime.Now.ToString() });
                        row.Elements.Add(new ListEntry.Custom() { LocalName = "lastname", Value = totalCredits.ToString() });

                        // Send the new row to the API for insertion.
                        gDocsService.Insert(listFeed, row);
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Couldn't write to Google: " + ex.Message);
                        SetupGoogle();
                    }
                }
                );

                NextLogToGoogleTime = DateTime.Now.AddMinutes(15 - ((DateTime.Now.Minute + 15) % 60 % 15));
                //Log.Info("NextLogToGoogleTime: " + NextLogToGoogleTime.ToString());
            }
        }

        private void LoadUserDetails()
        {
            using (StreamReader sr = new StreamReader("accounts.config"))
            {
                loginDetails = new Dictionary<string, UserDetails>();
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    if (str.StartsWith("--") || String.IsNullOrWhiteSpace(str)) continue;

                    string[] parts = str.Split(',');
                    loginDetails.Add(parts[0], new UserDetails()
                    {
                        LoginDetails = new LoginDetails(parts[1], parts[2], parts[3], Platform.Xbox360),
                        UnsellableItems = String.IsNullOrWhiteSpace(parts[4]) ?
                                                new List<long>() :
                                                parts[4].Split('|').Select(i => { return Int64.Parse(i); }).ToList<long>()
                    });
                }
            }
        }

        public List<string> GetAccountNames()
        {
            return loginDetails.Keys.ToList<string>();
        }

        private void RecordExceptionEvent()
        {
            if (DateTime.UtcNow > ExceptionEventWindowStart.AddMinutes(ExceptionEventWindowLengthMinutes))
            {
                ExceptionsEventDuringWindow = 1;
                ExceptionEventWindowStart = DateTime.UtcNow;
            }
            else
            {
                ExceptionsEventDuringWindow++;
            }
        }

        private bool TooManyExceptions()
        {
            return ExceptionsEventDuringWindow > ExceptionEventThreshold;
        }
    }

    public class UserDetails
    {
        public LoginDetails LoginDetails { get; set; }
        public List<long> UnsellableItems { get; set; }
    }
}
