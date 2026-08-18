export class OrderRequest {
  userID: string = "";
  addressID: string = "";
  paymentID: string = "";
  shippingCostID: string = "";
  orderCouponID: string | null = null;
  subtotal: number = 0;
  discountAmount: number = 0;
  totalAmount: number = 0;
}
