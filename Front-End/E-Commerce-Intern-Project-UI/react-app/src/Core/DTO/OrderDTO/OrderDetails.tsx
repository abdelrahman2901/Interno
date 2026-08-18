import { AddressDetails } from "../AddressDTO/AddressDetails";
import { OrderCouponModel } from "../OrdeCouponDTO/OrderCouponResponse";
import { OrderItemDetails } from "../OrderItemDTO/OrderItemDetails";
import { ShippingCostDetails } from "../ShippingCostDTO/ShippingCostDetails";

export class OrderDetails {
  orderID: string = "";
  orderItems: OrderItemDetails[] = [];
  address: AddressDetails = new AddressDetails();
  // payment: PaymentModel = new PaymentModel();
  userName: string = "";
  userEmail: string = "";
  paymentMethod: string = "";

  orderNumber: string = "";
  orderDate: string = "";
  subtotal: number = 0;
  shippingCosts: ShippingCostDetails = new ShippingCostDetails();
  discountAmount: number = 0;
  totalAmount: number = 0;
  orderCoupon: OrderCouponModel = new OrderCouponModel();
  orderStatus: string = "";
}
