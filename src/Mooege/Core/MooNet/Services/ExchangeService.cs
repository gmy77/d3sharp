/*
 * Copyright (C) 2011 mooege project
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
using Google.ProtocolBuffers;
using Mooege.Common;
using Mooege.Net.MooNet;
using bnet.protocol;
using bnet.protocol.exchange_object_provider;
using bnet.protocol.exchange_risk;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x0a, serviceName: "bnet.protocol.exchange.ExchangeService")]
    public class ExchangeService : bnet.protocol.exchange.ExchangeService, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }

        public override void GetConfiguration(IRpcController controller, bnet.protocol.exchange.GetConfigurationRequest request, Action<bnet.protocol.exchange.GetConfigurationResponse> done)
        {
            Logger.Trace("GetConfiguration()");
            //TODO: Figure out what the buyout rules/specialist values are, and if they are related /dustinconrad
            var builder = bnet.protocol.exchange.GetConfigurationResponse.CreateBuilder()
                .AddConfigs(bnet.protocol.exchange.SpecialistConfig.CreateBuilder()
                    .SetSpecialist(1)
                    .AddAuctionDurations(720)
                    .AddAuctionDurations(1440)
                    .AddAuctionDurations(2880)
                    .AddAuctionStartDelays(5)
                    .SetAntiSnipingExtensionDelay(1)
                    .AddCurrencyConfig(bnet.protocol.exchange.CurrencyConfig.CreateBuilder()
                        .SetCurrency("D3_GOLD")
                        .SetTickSize(1)
                        .SetMinUnitPrice(5)
                        .SetMaxUnitPrice(4294967295)
                        .SetMaxTotalPrice(4294967295)));
            done(builder.Build());
        }

        public override void SubscribeOrderStatusChange(IRpcController controller, bnet.protocol.exchange.SubscribeOrderStatusChangeRequest request, Action<NoData> done)
        {
            Logger.Trace("SubscribeOrderStatusChange() Stub");
            var builder = NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void UnsubscribeOrderStatusChange(IRpcController controller, bnet.protocol.exchange.UnsubscribeOrderStatusChangeRequest request, Action<NoData> done)
        {
            Logger.Trace("UnsubscribeOrderStatusChange() Stub");
            var builder = NoData.CreateBuilder();
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

        public override void ReportAuthorize(IRpcController controller, ReportAuthorizeRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void ReportSettle(IRpcController controller, ReportSettleRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void ReportCancel(IRpcController controller, ReportCancelRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void SubscribeOrderBookStatusChange(IRpcController controller, bnet.protocol.exchange.SubscribeOrderBookStatusChangeRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UnsubscribeOrderBookStatusChange(IRpcController controller, bnet.protocol.exchange.UnsubscribeOrderBookStatusChangeRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void GetPaymentMethods(IRpcController controller, GetPaymentMethodsRequest request, Action<GetPaymentMethodsResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void ClaimBidItem(IRpcController controller, bnet.protocol.exchange.ClaimRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void ClaimBidMoney(IRpcController controller, bnet.protocol.exchange.ClaimRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void ClaimOfferItem(IRpcController controller, bnet.protocol.exchange.ClaimRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void ClaimOfferMoney(IRpcController controller, bnet.protocol.exchange.ClaimRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void CancelBid(IRpcController controller, bnet.protocol.exchange.CancelRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void CancelOffer(IRpcController controller, bnet.protocol.exchange.CancelRequest request, Action<NoData> done)
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

        public override void ReportAuthorizeRiskVerdict(IRpcController controller, ReportAuthorizeRiskVerdictRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void ReportSettleRiskVerdict(IRpcController controller, ReportSettleRiskVerdictRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void DelaySettleRiskVerdict(IRpcController controller, DelaySettleRiskVerdictRequest request, Action<NoData> done)
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
    }
}
