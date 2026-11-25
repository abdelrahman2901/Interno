export async function requestPasswordReset(email: string): Promise<void> {
  void email;
}

export type ContactMessageRequest = {
  name: string;
  email: string;
  subject: string;
  message: string;
};

export async function sendContactMessage(
  payload: ContactMessageRequest,
): Promise<void> {
  void payload;
}
