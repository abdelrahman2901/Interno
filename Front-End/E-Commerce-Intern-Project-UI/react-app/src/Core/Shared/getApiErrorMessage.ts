import axios from "axios";
import { SwaggerException } from "../API/Interno-Microservice-Client/Interno-Microservice-Client";

function readServerMessage(payload: unknown): string | undefined {
  if (!payload) return undefined;
  if (typeof payload === "string") {
    const trimmed = payload.trim();
    if (!trimmed) return undefined;
    try {
      return readServerMessage(JSON.parse(trimmed));
    } catch {
      return trimmed;
    }
  }
  if (typeof payload === "object") {
    const data = payload as Record<string, unknown>;
    const nested =
      data.errorMessage ??
      data.message ??
      data.title ??
      data.detail ??
      data.error;
    if (typeof nested === "string" && nested.trim()) return nested.trim();
  }
  return undefined;
}

export function getApiErrorMessage(err: unknown): string {
  let status: number | undefined;
  let message: string | undefined;

  if (axios.isAxiosError(err)) {
    status = err.response?.status;
    message = readServerMessage(err.response?.data) ?? err.message;
  } else if (SwaggerException.isSwaggerException(err)) {
    status = err.status;
    message = readServerMessage(err.response) ?? err.message;
  } else if (err && typeof err === "object") {
    const extra = err as {
      status?: number;
      statusCode?: number;
      message?: string;
      response?: unknown;
      errorMessage?: string;
    };
    status = extra.status ?? extra.statusCode;
    message =
      readServerMessage(extra.response) ??
      extra.errorMessage ??
      extra.message;
  } else if (typeof err === "string") {
    message = err;
  }

  if (status === 500) return "Something went wrong";
  return message?.trim() || "Something went wrong";
}
