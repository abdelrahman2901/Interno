import { create } from "zustand";

type CartStore = {
  cartVersion: number;
  triggerCartUpdate: () => void;
};

export const useCartStore = create<CartStore>((set) => ({
  cartVersion: 0,
  triggerCartUpdate: () =>
    set((state) => ({
      cartVersion: state.cartVersion + 1,
    })),
}));
