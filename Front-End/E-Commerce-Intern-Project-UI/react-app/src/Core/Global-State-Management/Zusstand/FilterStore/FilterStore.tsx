import { create } from "zustand";
import { FilterPageModel } from "../../../../Features/Components/ProductPage-Components/Model/FilterPageModel";

type FilterStore = {
  filter: FilterPageModel;
  setFilter: (newFilter: Partial<FilterPageModel>) => void;
  resetFilter: () => void;
};

export const useFilterStore = create<FilterStore>((set) => ({
  filter: {
    Category: "",
    SubCategory: "",
    Size: "",
    Color: "",
    Sort: "",
    Price: 0,
  },

  setFilter: (newFilter) =>
    set((state) => ({
      filter: { ...state.filter, ...newFilter },
    })),

  resetFilter: () =>
    set({
      filter: {
        Category: "All",
        Color: "All",
        Sort: "All",
        SubCategory: "All",
        Size: "All",
        Price: 0,
      },
    }),
}));
