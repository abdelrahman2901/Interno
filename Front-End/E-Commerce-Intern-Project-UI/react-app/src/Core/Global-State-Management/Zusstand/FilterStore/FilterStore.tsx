import { create } from "zustand";
import { FilterPageModel } from "../../../../Features/Components/ProductPage-Components/Model/FilterPageModel";

type FilterStore = {
  filter: FilterPageModel;
  setFilter: (newFilter: Partial<FilterPageModel>) => void;
  resetFilter: () => void;
};

export const useFilterStore = create<FilterStore>((set) => ({
  filter: {
    CategoryID: undefined,
    SubCategoryID: undefined,
    SizeID: undefined,
    ColorID: undefined,
    Sort: '',
    Price: 0,
  },

  setFilter: (newFilter) =>
    set((state) => ({
      filter: { ...state.filter, ...newFilter },
    })),

  resetFilter: () =>
    set({
      filter: {
        CategoryID: undefined,
        SubCategoryID: undefined,
        SizeID: undefined,
        ColorID: undefined,
        Sort: "",
        Price: 0,
      },
    }),
}));
