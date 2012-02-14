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

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x0a, serviceName: "bnet.protocol.exchange.ExchangeService")]
    public class ExchangeService : bnet.protocol.exchange.ExchangeService, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }

        public override void GetConfiguration(IRpcController controller, bnet.protocol.exchange.GetConfigurationRequest request, Action<bnet.protocol.exchange.GetConfigurationResponse> done)
        {
            Logger.Trace("GetConfiguration()");
            //TODO: Figure out what the buyout rules/specialist values are, and if they are related /dustinconrad
            var builder = bnet.protocol.exchange.GetConfigurationResponse.CreateBuilder()
                .AddConfigs(bnet.protocol.exchange.SpecialistConfig.CreateBuilder()
                    .SetSpecialist(1)
                    //.AddAuctionDurations(720)
                    //.AddAuctionDurations(1440)
                    .AddAuctionDurations(2880)
                    .AddAuctionStartDelays(5)
                    .SetAntiSnipingExtensionDelay(1)
                    .SetMaxItemsAmount(1)
                    .SetStartingUnitPriceRule(2)
                    .SetReservedUnitPriceRule(1)
                    .SetTradeNowUnitPriceRule(1)
                    .SetCurrentUnitPriceRule(2)
                    .SetMaximumUnitPriceRule(2)
                    .AddCurrencyConfig(bnet.protocol.exchange.CurrencyConfig.CreateBuilder()
                        .SetCurrency("PTR")
                        .SetTickSize(1)
                        .SetMinTotalPrice(100)
                        .SetMinUnitPrice(100)
                        .SetMaxUnitPrice(4294967295)
                        .SetMaxTotalPrice(281474976710655).Build()))
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
                    .AddCurrencyConfig(bnet.protocol.exchange.CurrencyConfig.CreateBuilder()
                        .SetCurrency("PTR")
                        .SetTickSize(1)
                        .SetMinUnitPrice(100)
                        .SetMinTotalPrice(100)
                        .SetMaxUnitPrice(4294967295)
                        .SetMaxTotalPrice(281474976710655).Build()));

            done(builder.Build());
        }

        public override void SubscribeOrderStatusChange(IRpcController controller, bnet.protocol.exchange.SubscribeOrderStatusChangeRequest request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("SubscribeOrderStatusChange() Stub");
            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void UnsubscribeOrderStatusChange(IRpcController controller, bnet.protocol.exchange.UnsubscribeOrderStatusChangeRequest request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("UnsubscribeOrderStatusChange() Stub");
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
            var data = new byte[] { 0x6A, 0x04, 0x65, 0x6E, 0x55, 0x53, 0x7A, 0x0A, 0x42, 0x61, 0x74, 0x74, 0x6C, 0x65, 0x43, 0x6F, 0x69, 0x6E };
            //j\004enUSz\nBattleCoin
            //data is added to the end of extensionData
            var extensionData = bnet.protocol.exchange.Extension.CreateBuilder()
                .SetPartitionId(bnet.protocol.exchange.PartitionId.ParseFrom(this.Client.Account.BnetEntityId.ToByteArray()))
                .SetOrderBookId(1)
                .SetOrderId(3)
                .SetFilledAmount(0)
                .SetOrderStatus((int)this.Client.Account.BnetEntityId.Low)
                .SetAuthorizedTime(0)
                .Build();

            var method = bnet.protocol.exchange_object_provider.PaymentMethod.CreateBuilder()
                .SetAccount(bnet.protocol.exchange.BlobFrom.CreateBuilder()
                    .SetSource(1161969996)
                    .SetData(ByteString.CopyFrom(extensionData.ToByteArray().Concat(data).ToArray())))
                .SetDescription("BattleCoin")
                .SetAmount(100000)
                .SetCashInOutMask(3)
                .SetWalletId(0)
                .Build();
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
            throw new NotImplementedException();
        }
    }
}
