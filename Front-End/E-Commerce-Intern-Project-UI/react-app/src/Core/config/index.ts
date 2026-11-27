export interface AppConfig {
  gatewayUrl: string;
  filesBaseUrl: string;
  avatarApiUrl: string;
}

export const config: AppConfig = {
  gatewayUrl:
    import.meta.env.VITE_GATEWAY_URL ||
    (import.meta.env.PROD
      ? "https://internogateway.runasp.net/gateway"
      : "http://localhost:5003/gateway"),
  filesBaseUrl:
    import.meta.env.VITE_FILES_BASE_URL ||
    (import.meta.env.PROD
      ? "https://internoapi.runasp.net"
      : "http://localhost:5000"),
  avatarApiUrl:
    import.meta.env.VITE_AVATAR_API_URL || "https://ui-avatars.com/api",
};

export const getAvatarUrl = (
  name: string,
  background?: string,
  color?: string,
): string => {
  const params = new URLSearchParams({
    name,
    background: background || "8b6f5e",
    color: color || "fff",
  });
  return `${config.avatarApiUrl}/?${params.toString()}`;
};

const PRODUCT_IMAGES = [
  "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?auto=format&fit=crop&w=800&q=80",
  "https://images.unsplash.com/photo-1539109136881-3be0616ffa4a?auto=format&fit=crop&w=800&q=80",
  "https://images.unsplash.com/photo-1490481651871-ab68de25d43d?auto=format&fit=crop&w=800&q=80",
  "https://images.unsplash.com/photo-1525507119028-ed4c629a60a3?auto=format&fit=crop&w=800&q=80",
  "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=800&q=80",
  "https://images.unsplash.com/photo-1558769132-cb1aea458c5e?auto=format&fit=crop&w=800&q=80",
  "https://images.unsplash.com/photo-1441984904996-e0b6ba687e04?auto=format&fit=crop&w=800&q=80",
  "https://images.unsplash.com/photo-1469334031218-e382a71b716b?auto=format&fit=crop&w=800&q=80",
  "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?auto=format&fit=crop&w=800&q=80",
  "https://images.unsplash.com/photo-1434389677669-e08b4cac3105?auto=format&fit=crop&w=800&q=80",
  "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=800&q=80",
  "https://images.unsplash.com/photo-1617137968427-85924c800a22?auto=format&fit=crop&w=800&q=80",
];

const BANNER_IMAGES = [
  "https://images.unsplash.com/photo-1441986300917-64674bd600d8?auto=format&fit=crop&w=1600&q=80",
  "https://images.unsplash.com/photo-1445205170230-053b83016050?auto=format&fit=crop&w=1600&q=80",
  "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=1600&q=80",
  "https://images.unsplash.com/photo-1490481651871-ab68de25d43d?auto=format&fit=crop&w=1600&q=80",
];

const CATEGORY_IMAGES = [
  "https://images.unsplash.com/photo-1490481651871-ab68de25d43d?auto=format&fit=crop&w=800&h=800&q=80",
  "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?auto=format&fit=crop&w=800&h=800&q=80",
  "https://images.unsplash.com/photo-1441984904996-e0b6ba687e04?auto=format&fit=crop&w=800&h=800&q=80",
  "https://images.unsplash.com/photo-1525507119028-ed4c629a60a3?auto=format&fit=crop&w=800&h=800&q=80",
  "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?auto=format&fit=crop&w=800&h=800&q=80",
  "https://images.unsplash.com/photo-1434389677669-e08b4cac3105?auto=format&fit=crop&w=800&h=800&q=80",
];

function pickImage(seed: string | undefined, images: string[]) {
  const key = seed?.trim() || "placeholder";
  let hash = 0;
  for (let i = 0; i < key.length; i++) {
    hash = (hash << 5) - hash + key.charCodeAt(i);
    hash |= 0;
  }
  return images[Math.abs(hash) % images.length];
}

export const getProductImageUrl = (seed?: string) =>
  pickImage(seed, PRODUCT_IMAGES);

export const getBannerImageUrl = (seed?: string) =>
  pickImage(seed, BANNER_IMAGES);

export const getCategoryImageUrl = (seed?: string) =>
  pickImage(seed, CATEGORY_IMAGES);
