import React, { useEffect, useState } from "react";
import { OrderDetails } from "../../../../../Core/DTO/OrderDTO/OrderDetails";
import { GetAllUserOrders } from "../../../../../Core/Services/OrderServices/OrderService";
import { useAuth } from "../../../../../Core/Services/AuthServices/AuthProvider";
import { ProductRatesRequets } from "../../../../../Core/DTO/ProductRatesDTO/ProductRatesRequest";
import { CreateProductRateList } from "../../../../../Core/Services/ProductRatingServices/ProductRatingService";
import SuccessRateModal from "./SuccessRateModal";

type props = {
  orderIDProps: string | null;
  onClose: () => void;
  onActionResult: () => void;
};
const Base_Url = "https://localhost:7164";

export default function RatingModal({
  orderIDProps,
  onClose,
  onActionResult,
}: props) {
  const [_orders, setPrivOrders] = useState<OrderDetails[]>([]);
  const [order, setOrders] = useState<OrderDetails>(new OrderDetails());
  const { user } = useAuth();
  const [ratings, setRatings] = useState<Record<string, number>>({});

  const laodOrders = async () => {
    try {
      const response = await GetAllUserOrders(user?.userID!);
      if (response.isSuccess) {
        setPrivOrders(response.data!);
      }
    } catch (err) {
      if (err) {
        console.error(err);
      }
    }
  };
  useEffect(() => {
    if (_orders.length === 0) {
      laodOrders();
    }
  }, []);
  useEffect(() => {
    console.log(orderIDProps);

    if (orderIDProps && _orders.length > 0) {
      setOrders(_orders.find((r) => r.orderID === orderIDProps)!);
    }
  }, [orderIDProps, _orders.length]);

  async function CreateRate() {
    const request: ProductRatesRequets[] = [];
    for (let productID in ratings) {
      request.push({
        rating: ratings[productID] as 1 | 2 | 3 | 4 | 5,
        productID: productID,
        userID: user?.userID!,
      });
    }

    try {
      const response = await CreateProductRateList(request);
      if (response.isSuccess) {
        onActionResult();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  }

  function placeRating(rateValue: number, productID: string) {
    setRatings((prev) => ({
      ...prev,
      [productID]: rateValue,
    }));
  }
  function updateRatingText(productID: string) {
    let ratingText;
    const value: 1 | 2 | 3 | 4 | 5 = ratings[productID] as 1 | 2 | 3 | 4 | 5;
    const ratingTexts = {
      1: "⭐ Poor",
      2: "⭐⭐ Fair",
      3: "⭐⭐⭐ Good",
      4: "⭐⭐⭐⭐ Very Good",
      5: "⭐⭐⭐⭐⭐ Excellent",
    };
    ratingText = ratingTexts[value];

    return ratingText;
  }

  return (
    <>
      <div className="rating-modal">
        <div className="rating-modal-content">
          <div className="rating-modal-header">
            <h3>Rate Products</h3>
            <button className="rating-modal-close" onClick={onClose}>
              &times;
            </button>
          </div>
          <div className="rating-modal-body">
            {order.orderItems.map((orderItem) => (
              <>
                <div className="rating-product-info" key={orderItem.orderID}>
                  <img
                    src={`${Base_Url}/ProductsImages/${orderItem.product.parentCategoryName}/${orderItem.product.categoryName}/${orderItem.product.productImageUrl}`}
                    alt="Product"
                  />
                  <div className="rating-product-details">
                    <h4>{orderItem.product.productName}</h4>
                    <p>{order.orderNumber}</p>
                  </div>
                </div>

                <div className="rating-section">
                  <label className="rating-label">Your Rating *</label>
                  <div className="star-rating input">
                    {[5, 4, 3, 2, 1].map((value) => (
                      <>
                        <label
                          className={`star ${ratings[orderItem.product.productID] >= value ? "checked" : ""}`}
                        >
                          <input
                            type="radio"
                            name={`rating-${orderItem.product.productID}`}
                            value={value}
                            onChange={(e) => {
                              console.log("wefwwes");

                              const rateValue = Number(e.currentTarget.value);
                              placeRating(
                                rateValue,
                                orderItem.product.productID,
                              );
                            }}
                            checked={
                              ratings[orderItem.product.productID] === value
                            }
                          />
                          ★
                        </label>
                      </>
                    ))}
                  </div>
                  <span className="rateText">
                    {updateRatingText(orderItem.product.productID)}
                  </span>
                  <p className="rating-text">Select a rating</p>
                </div>
              </>
            ))}
            <div className="rating-section">
              <label className="checkbox-label">
                <input type="checkbox" />
                <span>
                  I agree to the <a href="#">Terms & Conditions</a> and confirm
                  this review is based on my own experience.
                </span>
              </label>
            </div>

            <div className="rating-actions">
              <button type="button" className="btn-cancel" onClick={onClose}>
                Cancel
              </button>
              <button
                type="button"
                className="btn-submit-rating"
                onClick={CreateRate}
              >
                Submit Review
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
