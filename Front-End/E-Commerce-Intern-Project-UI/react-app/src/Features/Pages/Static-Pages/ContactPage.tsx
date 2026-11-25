import { useState } from "react";
import { Clock, Mail, MapPin, Phone } from "lucide-react";
import PageLayout, { PageCrumb } from "./PageLayout";
import { getApiErrorMessage } from "../../../Core/Shared/getApiErrorMessage";
import { sendContactMessage } from "../../../Core/Shared/AuthApiStubs";

type ContactCard = {
  title: string;
  body: string;
  extra?: string;
  icon: "mail" | "phone" | "pin" | "clock";
};

type SubjectOption = {
  value: string;
  label: string;
};

const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

const SUBJECTS: SubjectOption[] = [
  { value: "order", label: "Order support" },
  { value: "return", label: "Return or exchange" },
  { value: "product", label: "Product question" },
  { value: "other", label: "Something else" },
];

const CONTACT_CARDS: ContactCard[] = [
  {
    icon: "mail",
    title: "Email",
    body: "hello@interno.example",
    extra: "We reply on business days.",
  },
  {
    icon: "phone",
    title: "Phone",
    body: "+20 100 000 0000",
    extra: "Sun-Thu, 10:00-18:00",
  },
  {
    icon: "pin",
    title: "Studio",
    body: "14 Nile Atelier Lane, Cairo",
    extra: "Visits by appointment.",
  },
  {
    icon: "clock",
    title: "Working hours",
    body: "Sunday to Thursday",
    extra: "10:00-18:00 Cairo time",
  },
];

const ICONS = {
  mail: Mail,
  phone: Phone,
  pin: MapPin,
  clock: Clock,
};

const CRUMBS: PageCrumb[] = [
  { label: "Home", to: "/Home" },
  { label: "Contact" },
];

export default function ContactPage() {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [subject, setSubject] = useState("");
  const [message, setMessage] = useState("");
  const [submitted, setSubmitted] = useState(false);
  const [isPending, setIsPending] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false);

  const nameError = submitted && !name.trim() ? "Name cant be empty" : "";
  const emailError =
    submitted && !EMAIL_PATTERN.test(email.trim())
      ? "Use a valid email format"
      : "";
  const subjectError = submitted && !subject ? "Choose a subject" : "";
  const messageError =
    submitted && message.trim().length < 10
      ? "Message must be at least 10 characters"
      : "";

  function isValid() {
    return (
      name.trim().length > 0 &&
      EMAIL_PATTERN.test(email.trim()) &&
      subject &&
      message.trim().length >= 10
    );
  }

  async function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setSubmitted(true);
    setError("");
    setSuccess(false);
    if (!isValid()) return;

    setIsPending(true);
    try {
      await sendContactMessage({
        name: name.trim(),
        email: email.trim(),
        subject,
        message: message.trim(),
      });
      setSuccess(true);
    } catch (err) {
      setError(getApiErrorMessage(err));
    } finally {
      setIsPending(false);
    }
  }

  return (
    <PageLayout
      title="Contact"
      subtitle="Questions about a fit, an order, or a collaboration? Write to the studio and we will get back to you."
      crumbs={CRUMBS}
    >
      <section className="contact-layout">
        <form className="info-card contact-form" onSubmit={onSubmit} noValidate>
          {success ? (
            <div className="form-success" role="status">
              <p>
                Thanks {name.trim()}. We have received your message and will
                reply to {email.trim()} shortly.
              </p>
            </div>
          ) : (
            <>
              <label>
                Name
                <input
                  type="text"
                  name="name"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  autoComplete="name"
                />
                {nameError && <span className="field-error">{nameError}</span>}
              </label>

              <label>
                Email
                <input
                  type="email"
                  name="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  autoComplete="email"
                />
                {emailError && <span className="field-error">{emailError}</span>}
              </label>

              <label>
                Subject
                <select
                  name="subject"
                  value={subject}
                  onChange={(e) => setSubject(e.target.value)}
                >
                  <option value="">Select a topic</option>
                  {SUBJECTS.map((option) => (
                    <option key={option.value} value={option.value}>
                      {option.label}
                    </option>
                  ))}
                </select>
                {subjectError && (
                  <span className="field-error">{subjectError}</span>
                )}
              </label>

              <label>
                Message
                <textarea
                  name="message"
                  value={message}
                  onChange={(e) => setMessage(e.target.value)}
                />
                {messageError && (
                  <span className="field-error">{messageError}</span>
                )}
              </label>

              {error && (
                <span className="field-error" role="alert">
                  {error}
                </span>
              )}

              <button type="submit" className="info-btn" disabled={isPending}>
                {isPending ? "Sending..." : "Send message"}
              </button>
            </>
          )}
        </form>

        <aside>
          <div className="info-grid">
            {CONTACT_CARDS.map((card) => {
              const Icon = ICONS[card.icon];
              return (
                <article className="info-card" key={card.title}>
                  <span className="info-icon">
                    <Icon size={18} aria-hidden="true" />
                  </span>
                  <h3>{card.title}</h3>
                  <p>{card.body}</p>
                  {card.extra && <p>{card.extra}</p>}
                </article>
              );
            })}
          </div>
        </aside>
      </section>

      <section className="info-section" style={{ marginTop: 28 }}>
        <div className="map-placeholder" aria-label="Map placeholder">
          <div>
            <strong>Studio map</strong>
            <p>Interactive map will sit here once the location API is wired.</p>
          </div>
        </div>
      </section>
    </PageLayout>
  );
}
