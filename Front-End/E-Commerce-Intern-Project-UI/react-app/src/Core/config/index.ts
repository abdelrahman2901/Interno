export interface AppConfig {
  avatarApiUrl: string;
}

export const config: AppConfig = {
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
