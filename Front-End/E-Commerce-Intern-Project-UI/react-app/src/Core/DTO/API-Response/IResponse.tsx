export interface IResponse<T> {
  data?: T;
  isSuccess: boolean;
  statusCode?: number;
  errorMessage?: string;
}
