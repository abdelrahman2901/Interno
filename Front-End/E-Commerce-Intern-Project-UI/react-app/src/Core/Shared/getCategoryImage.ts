type CategoryLike = {
  categoryName?: string | undefined;
};

const WOMEN_IMAGES = [
  "https://images.unsplash.com/photo-1490481651871-ab68de25d43d?auto=format&fit=crop&w=800&h=1000&q=80",
  "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?auto=format&fit=crop&w=800&h=1000&q=80",
  "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=800&h=1000&q=80",
  "https://images.unsplash.com/photo-1469334031218-e382a71b716b?auto=format&fit=crop&w=800&h=1000&q=80",
];

const MEN_IMAGES = [
  "https://images.unsplash.com/photo-1617137968427-85924c800a22?auto=format&fit=crop&w=800&h=1000&q=80",
  "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=800&h=1000&q=80",
  "https://images.unsplash.com/photo-1488161628813-04466f872be2?auto=format&fit=crop&w=800&h=1000&q=80",
  "https://images.unsplash.com/photo-1506794778202-cad84cf45f1d?auto=format&fit=crop&w=800&h=1000&q=80",
];

const KIDS_IMAGES = [
  "https://images.unsplash.com/photo-1503919545889-aef636e10ad4?auto=format&fit=crop&w=800&h=1000&q=80",
  "https://images.unsplash.com/photo-1519238263530-99bdd11df2ea?auto=format&fit=crop&w=800&h=1000&q=80",
  "https://images.unsplash.com/photo-1503454537195-1dcabb73ffb9?auto=format&fit=crop&w=800&h=1000&q=80",
  "https://images.unsplash.com/photo-1522771739844-6a9f6d5f14af?auto=format&fit=crop&w=800&h=1000&q=80",
];

const ACCESSORY_IMAGES = [
  "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?auto=format&fit=crop&w=800&h=1000&q=80",
  "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?auto=format&fit=crop&w=800&h=1000&q=80",
];

const HOME_IMAGES = [
  "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?auto=format&fit=crop&w=800&h=1000&q=80",
  "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?auto=format&fit=crop&w=800&h=1000&q=80",
];

const FALLBACK_IMAGES = [
  "https://images.unsplash.com/photo-1441984904996-e0b6ba687e04?auto=format&fit=crop&w=800&h=1000&q=80",
  "https://images.unsplash.com/photo-1445205170230-053b83016050?auto=format&fit=crop&w=800&h=1000&q=80",
  "https://images.unsplash.com/photo-1558769132-cb1aea458c5e?auto=format&fit=crop&w=800&h=1000&q=80",
  "https://images.unsplash.com/photo-1525507119028-ed4c629a60a3?auto=format&fit=crop&w=800&h=1000&q=80",
];

function categoryName(category: CategoryLike | string | undefined) {
  if (!category) return "";
  return typeof category === "string" ? category : category.categoryName ?? "";
}

function poolFor(name: string) {
  const key = name.toLowerCase();
  if (key.includes("women") || key.includes("woman") || key.includes("lady")) {
    return WOMEN_IMAGES;
  }
  if (key.includes("men") || key.includes("man") || key.includes("male")) {
    return MEN_IMAGES;
  }
  if (key.includes("kid") || key.includes("child") || key.includes("baby")) {
    return KIDS_IMAGES;
  }
  if (key.includes("accessor") || key.includes("bag") || key.includes("watch")) {
    return ACCESSORY_IMAGES;
  }
  if (key.includes("home") || key.includes("furniture") || key.includes("living")) {
    return HOME_IMAGES;
  }
  return FALLBACK_IMAGES;
}

export function getCategoryImage(
  category: CategoryLike | string | undefined,
  index: number,
) {
  const pool = poolFor(categoryName(category));
  const safeIndex = Math.max(0, index);
  return pool[safeIndex % pool.length];
}
