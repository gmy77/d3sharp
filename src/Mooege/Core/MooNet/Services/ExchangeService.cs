/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Linq;
using Google.ProtocolBuffers;
using Mooege.Common.Logging;
using Mooege.Net.MooNet;
using Mooege.Core.MooNet.Helpers;
using Mooege.Common.Extensions;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x0a, serviceName: "bnet.protocol.exchange.ExchangeService")]
    public class ExchangeService : bnet.protocol.exchange.ExchangeService, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }
        public uint Status { get; set; }

        public override void GetConfiguration(IRpcController controller, bnet.protocol.exchange.GetConfigurationRequest request, Action<bnet.protocol.exchange.GetConfigurationResponse> done)
        {
            Logger.Trace("GetConfiguration()");
            //TODO: Figure out what the buyout rules/specialist values are, and if they are related /dustinconrad
            var builder = bnet.protocol.exchange.GetConfigurationResponse.CreateBuilder()
                .AddConfigs(bnet.protocol.exchange.SpecialistConfig.CreateBuilder()
                    .SetSpecialist(1)
                    .AddAuctionDurations(2880)
                    .AddAuctionStartDelays(5)
                    .SetAntiSnipingExtensionDelay(1)
                    .SetMaxItemsAmount(1)
                    .SetStartingUnitPriceRule(2)
                    .SetReservedUnitPriceRule(1)
                    .SetTradeNowUnitPriceRule(1)
                    .SetCurrentUnitPriceRule(2)
                    .SetMaximumUnitPriceRule(2)
                    .SetFillOrKillRule(0)
                    .AddCurrencyConfig(bnet.protocol.exchange.CurrencyConfig.CreateBuilder()
                        .SetCurrency("D3_GOLD")
                        .SetTickSize(1)
                        .SetMinTotalPrice(100)
                        .SetMinUnitPrice(100)
                        .SetMaxUnitPrice(100000000000)
                        .SetMaxTotalPrice(100000000000).Build())
                    .AddCurrencyConfig(bnet.protocol.exchange.CurrencyConfig.CreateBuilder()
                        .SetCurrency("D3_GOLD_HC")
                        .SetTickSize(1)
                        .SetMinTotalPrice(100)
                        .SetMinUnitPrice(100)
                        .SetMaxUnitPrice(100000000000)
                        .SetMaxTotalPrice(100000000000).Build()))
                .AddConfigs(bnet.protocol.exchange.SpecialistConfig.CreateBuilder()
                    .SetSpecialist(2)
                    .AddAuctionDurations(2880)
                    .AddAuctionStartDelays(0)
                    .SetAntiSnipingExtensionDelay(0)
                    .SetMaxItemsAmount(4294967295)
                    .SetStartingUnitPriceRule(1)
                    .SetReservedUnitPriceRule(2)
                    .SetTradeNowUnitPriceRule(0)
                    .SetCurrentUnitPriceRule(0)
                    .SetMaximumUnitPriceRule(2)
                    .SetFillOrKillRule(1)
                    .AddCurrencyConfig(bnet.protocol.exchange.CurrencyConfig.CreateBuilder()
                        .SetCurrency("D3_GOLD")
                        .SetTickSize(1)
                        .SetMinTotalPrice(100)
                        .SetMinUnitPrice(100)
                        .SetMaxUnitPrice(100000000000)
                        .SetMaxTotalPrice(100000000000).Build())
                    .AddCurrencyConfig(bnet.protocol.exchange.CurrencyConfig.CreateBuilder()
                        .SetCurrency("D3_GOLD_HC")
                        .SetTickSize(1)
                        .SetMinTotalPrice(100)
                        .SetMinUnitPrice(100)
                        .SetMaxUnitPrice(100000000000)
                        .SetMaxTotalPrice(100000000000).Build()))
                .SetRecommendedDefaultRmtCurrency("USD")
                .SetRmtRestrictedByLicense(bnet.protocol.account.AccountLicense.CreateBuilder().SetId(222).SetExpires(1337724000000000));

            done(builder.Build());
        }

        public override void SubscribeOrderUpdate(IRpcController controller, bnet.protocol.exchange.SubscribeOrderUpdateRequest request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("SubscribeOrderUpdate() Stub");
            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void UnsubscribeOrderUpdate(IRpcController controller, bnet.protocol.exchange.UnsubscribeOrderUpdateRequest request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("UnsubscribeOrderUpdate() Stub");
            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void CreateOrderBook(IRpcController controller, bnet.protocol.exchange.CreateOrderBookRequest request, Action<bnet.protocol.exchange.CreateOrderBookResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void PlaceOfferOnOrderBook(IRpcController controller, bnet.protocol.exchange.PlaceOfferOnOrderBookRequest request, Action<bnet.protocol.exchange.PlaceOfferOnOrderBookResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void PlaceOfferCreateOrderBookIfNeeded(IRpcController controller, bnet.protocol.exchange.PlaceOfferCreateOrderBookIfNeededRequest request, Action<bnet.protocol.exchange.PlaceOfferCreateOrderBookIfNeededResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void PlaceBidOnOrderBook(IRpcController controller, bnet.protocol.exchange.PlaceBidOnOrderBookRequest request, Action<bnet.protocol.exchange.PlaceBidOnOrderBookResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void PlaceBidCreateOrderBookIfNeeded(IRpcController controller, bnet.protocol.exchange.PlaceBidCreateOrderBookIfNeededRequest request, Action<bnet.protocol.exchange.PlaceBidCreateOrderBookIfNeededResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void QueryOffersByOrderBook(IRpcController controller, bnet.protocol.exchange.QueryOffersByOrderBookRequest request, Action<bnet.protocol.exchange.QueryOffersByOrderBookResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void QueryBidsByOrderBook(IRpcController controller, bnet.protocol.exchange.QueryBidsByOrderBookRequest request, Action<bnet.protocol.exchange.QueryBidsByOrderBookResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void QueryOffersByAccountForItem(IRpcController controller, bnet.protocol.exchange.QueryOffersByAccountForItemRequest request, Action<bnet.protocol.exchange.QueryOffersByAccountForItemResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void QueryBidsByAccountForItem(IRpcController controller, bnet.protocol.exchange.QueryBidsByAccountForItemRequest request, Action<bnet.protocol.exchange.QueryBidsByAccountForItemResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void QueryOrderBooksSummary(IRpcController controller, bnet.protocol.exchange.QueryOrderBooksSummaryRequest request, Action<bnet.protocol.exchange.QueryOrderBooksSummaryResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void ReportAuthorize(IRpcController controller, bnet.protocol.exchange_object_provider.ReportAuthorizeRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void ReportSettle(IRpcController controller, bnet.protocol.exchange_object_provider.ReportSettleRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void ReportCancel(IRpcController controller, bnet.protocol.exchange_object_provider.ReportCancelRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void SubscribeOrderBookStatusChange(IRpcController controller, bnet.protocol.exchange.SubscribeOrderBookStatusChangeRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UnsubscribeOrderBookStatusChange(IRpcController controller, bnet.protocol.exchange.UnsubscribeOrderBookStatusChangeRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void GetPaymentMethods(IRpcController controller, bnet.protocol.exchange_object_provider.GetPaymentMethodsRequest request, Action<bnet.protocol.exchange_object_provider.GetPaymentMethodsResponse> done)
        {
            Logger.Trace("GetPaymentMethods()");

            var builder = bnet.protocol.exchange_object_provider.GetPaymentMethodsResponse.CreateBuilder();
            var method = bnet.protocol.exchange_object_provider.PaymentMethod.CreateBuilder();

            switch (request.Currency)
            {
                case "D3_GOLD":
                case "D3_GOLD_HC":
                    var goldExtensionData = "{ kind: GAME_ACCOUNT (2)  id: " + this.Client.Account.CurrentGameAccount.PersistentID + " program: 17459 region: 98 }";

                    method.SetAccount(bnet.protocol.exchange.BlobFrom.CreateBuilder()
                            .SetSource((uint)FieldKeyHelper.Program.D3)
                            .SetData(ByteString.CopyFromUtf8(goldExtensionData)))
                        .SetDescription(request.Currency)
                        .SetAmount(100000)
                        .SetWalletId(0)
                        .Build();
                    break;
                case "PTR":
                    var data = new byte[] { 0x6A, 0x04, 0x65, 0x6E, 0x55, 0x53, 0x7A, 0x0A, 0x42, 0x61, 0x74, 0x74, 0x6C, 0x65, 0x43, 0x6F, 0x69, 0x6E };
                    //j\004enUSz\nBattleCoin
                    //data is added to the end of extensionData
                    var ptrExtensionData = bnet.protocol.exchange.Extension.CreateBuilder()
                        .SetPartitionId(bnet.protocol.exchange.PartitionId.ParseFrom(this.Client.Account.BnetEntityId.ToByteArray()))
                        .SetOrderBookId(1)
                        .SetOrderId(3)
                        .SetFilledAmount(0)
                        .SetOrderStatus((int)this.Client.Account.BnetEntityId.Low)
                        .SetAuthorizedTime(0)
                        .Build();

                    method.SetAccount(bnet.protocol.exchange.BlobFrom.CreateBuilder()
                            .SetSource((uint)FieldKeyHelper.Program.D3)
                            .SetData(ByteString.CopyFrom(ptrExtensionData.ToByteArray().Concat(data).ToArray())))
                        .SetDescription("BattleCoin")
                        .SetAmount(100000)
                        .SetCashInOutMask(3)
                        .SetBillingAddress(bnet.protocol.exchange.BillingAddress.CreateBuilder()
                            .SetCountryId(221)
                            .SetCity("Irvine")
                            .SetState("CA")
                            .SetPostalCode("92618"))
                        .SetWalletId(123456)
                        .SetCapRestriction(1)
                        .SetAuthenticatorCap(4294967295)
                        .SetSoftCap(4294967295)
                        .Build();
                    break;
            }

            builder.AddMethods(method);

            done(builder.Build());
        }

        public override void ClaimBidItem(IRpcController controller, bnet.protocol.exchange.ClaimRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void ClaimBidMoney(IRpcController controller, bnet.protocol.exchange.ClaimRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void ClaimOfferItem(IRpcController controller, bnet.protocol.exchange.ClaimRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void ClaimOfferMoney(IRpcController controller, bnet.protocol.exchange.ClaimRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void CancelBid(IRpcController controller, bnet.protocol.exchange.CancelRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void CancelOffer(IRpcController controller, bnet.protocol.exchange.CancelRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void GetBidFeeEstimation(IRpcController controller, bnet.protocol.exchange.GetBidFeeEstimationRequest request, Action<bnet.protocol.exchange.GetFeeEstimationResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void GetOfferFeeEstimation(IRpcController controller, bnet.protocol.exchange.GetOfferFeeEstimationRequest request, Action<bnet.protocol.exchange.GetFeeEstimationResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void QueryOrdersByAccountForItem(IRpcController controller, bnet.protocol.exchange.QueryOrdersByAccountForItemRequest request, Action<bnet.protocol.exchange.QueryOrdersByAccountForItemResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void ReportAuthorizeRiskVerdict(IRpcController controller, bnet.protocol.exchange_risk.ReportAuthorizeRiskVerdictRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void ReportSettleRiskVerdict(IRpcController controller, bnet.protocol.exchange_risk.ReportSettleRiskVerdictRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void DelaySettleRiskVerdict(IRpcController controller, bnet.protocol.exchange_risk.DelaySettleRiskVerdictRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void GetFeeDetails(IRpcController controller, bnet.protocol.exchange.GetFeeDetailsRequest request, Action<bnet.protocol.exchange.GetFeeDetailsResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void GetOrderBookStatistics(IRpcController controller, bnet.protocol.exchange.GetOrderBookStatisticsRequest request, Action<bnet.protocol.exchange.GetOrderBookStatisticsResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void ReportRefund(IRpcController controller, bnet.protocol.exchange_object_provider.ReportRefundRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void RefundBid(IRpcController controller, bnet.protocol.exchange.RefundRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void CreateCSTrade(IRpcController controller, bnet.protocol.exchange.CreateCSTradeRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void GetBidDetails(IRpcController controller, bnet.protocol.exchange.GetBidDetailsRequest request, Action<bnet.protocol.exchange.GetBidDetailsResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void GetOfferDetails(IRpcController controller, bnet.protocol.exchange.GetOfferDetailsRequest request, Action<bnet.protocol.exchange.GetOfferDetailsResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void GetSystemTime(IRpcController controller, bnet.protocol.NoData request, Action<bnet.protocol.exchange.GetSystemTimeResponse> done)
        {
            var builder = bnet.protocol.exchange.GetSystemTimeResponse.CreateBuilder()
                .SetTime(DateTime.Now.ToExtendedEpoch());

            done(builder.Build());
        }

        public override void GetOrderCount(IRpcController controller, bnet.protocol.exchange.GetOrderCountRequest request, Action<bnet.protocol.exchange.GetOrderCountResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void SubscribeAdvancedOrderUpdate(IRpcController controller, bnet.protocol.exchange.SubscribeAdvancedOrderUpdateRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UnsubscribeAdvancedOrderUpdate(IRpcController controller, bnet.protocol.exchange.UnsubscribeAdvancedOrderUpdateRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void SettleHistoriesForCS(IRpcController controller, bnet.protocol.exchange.HistoriesForCSRequest request, Action<bnet.protocol.exchange.HistoriesForCSResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void CancelHistoriesForCS(IRpcController controller, bnet.protocol.exchange.HistoriesForCSRequest request, Action<bnet.protocol.exchange.HistoriesForCSResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void CreateCSTradeItem(IRpcController controller, bnet.protocol.exchange.CreateCSTradeItemRequest request, Action<bnet.protocol.exchange.CreateCSTradeResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void CreateCSTradeMoney(IRpcController controller, bnet.protocol.exchange.CreateCSTradeMoneyRequest request, Action<bnet.protocol.exchange.CreateCSTradeResponse> done)
        {
            throw new NotImplementedException();
        }
    }
}
