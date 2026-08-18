import { AreaModel } from "../AreaDTO/AreaModel";

export class ShippingCostDetails {
  shippingCostID: string = "";
  shippingCost: number = 0;
  area: AreaModel = new AreaModel();
  isDeleted: boolean = false;
}
