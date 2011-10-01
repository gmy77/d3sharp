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
using bnet.protocol;
using bnet.protocol.exchange;
using bnet.protocol.exchange_object_provider;

namespace Extractor.Services
{
    [Service(serviceID: 0x0a, serviceName: "bnet.protocol.exchange.ExchangeService")]
    public class ExchangeService : bnet.protocol.exchange.ExchangeService
    {
        public override void CreateOrderBook(IRpcController controller, CreateOrderBookRequest request, Action<CreateOrderBookResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void PlaceOfferOnOrderBook(IRpcController controller, PlaceOfferOnOrderBookRequest request, Action<PlaceOfferOnOrderBookResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void PlaceOfferCreateOrderBookIfNeeded(IRpcController controller, PlaceOfferCreateOrderBookIfNeededRequest request, Action<PlaceOfferCreateOrderBookIfNeededResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void PlaceBidOnOrderBook(IRpcController controller, PlaceBidOnOrderBookRequest request, Action<PlaceBidOnOrderBookResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void PlaceBidCreateOrderBookIfNeeded(IRpcController controller, PlaceBidCreateOrderBookIfNeededRequest request, Action<PlaceBidCreateOrderBookIfNeededResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void QueryOffersByOrderBook(IRpcController controller, QueryOffersByOrderBookRequest request, Action<QueryOffersByOrderBookResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void QueryBidsByOrderBook(IRpcController controller, QueryBidsByOrderBookRequest request, Action<QueryBidsByOrderBookResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void QueryOffersByAccountForItem(IRpcController controller, QueryOffersByAccountForItemRequest request, Action<QueryOffersByAccountForItemResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void QueryBidsByAccountForItem(IRpcController controller, QueryBidsByAccountForItemRequest request, Action<QueryBidsByAccountForItemResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void QueryOrderBooksSummary(IRpcController controller, QueryOrderBooksSummaryRequest request, Action<QueryOrderBooksSummaryResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void QuerySettlementsByOrderBook(IRpcController controller, QuerySettlementsByOrderBookRequest request, Action<QuerySettlementsByOrderBookResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void ReportAuthorize(IRpcController controller, ReportAuthorizeRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void ReportSettle(IRpcController controller, ReportSettleRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void ReportCancel(IRpcController controller, ReportCancelRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void SubscribeOrderBookStatusChange(IRpcController controller, SubscribeOrderBookStatusChangeRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void UnsubscribeOrderBookStatusChange(IRpcController controller, UnsubscribeOrderBookStatusChangeRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void SubscribeOrderStatusChange(IRpcController controller, SubscribeOrderStatusChangeRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void UnsubscribeOrderStatusChange(IRpcController controller, UnsubscribeOrderStatusChangeRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void GetPaymentMethods(IRpcController controller, GetPaymentMethodsRequest request, Action<GetPaymentMethodsResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void ClaimBidItem(IRpcController controller, ClaimRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void ClaimBidMoney(IRpcController controller, ClaimRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void ClaimOfferItem(IRpcController controller, ClaimRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void ClaimOfferMoney(IRpcController controller, ClaimRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void CancelBid(IRpcController controller, CancelRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void CancelOffer(IRpcController controller, CancelRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void GetConfiguration(IRpcController controller, GetConfigurationRequest request, Action<GetConfigurationResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void GetBidFeeEstimation(IRpcController controller, GetBidFeeEstimationRequest request, Action<GetFeeEstimationResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void GetOfferFeeEstimation(IRpcController controller, GetOfferFeeEstimationRequest request, Action<GetFeeEstimationResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }
    }
}
