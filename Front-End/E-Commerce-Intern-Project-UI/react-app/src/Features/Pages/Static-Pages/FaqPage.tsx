import { useMemo, useState } from "react";
import { Link } from "react-router-dom";
import { ChevronDown, Search } from "lucide-react";
import PageLayout, { PageCrumb } from "./PageLayout";

type FaqCategory = "Orders" | "Payments" | "Account" | "Shipping";

type FaqItem = {
  id: string;
  category: FaqCategory;
  question: string;
  answer: string;
};

const CATEGORIES: FaqCategory[] = ["Orders", "Payments", "Account", "Shipping"];

const FAQ_ITEMS: FaqItem[] = [
  {
    id: "orders-1",
    category: "Orders",
    question: "How do I place an order?",
    answer:
      "Browse the shop, add pieces to your cart, then checkout as a guest or with your Interno account. You will receive an order confirmation email as soon as payment is accepted.",
  },
  {
    id: "orders-2",
    category: "Orders",
    question: "Can I change or cancel an order?",
    answer:
      "If the order has not yet been packed, we can usually update the size, address, or cancel it. Contact us within 2 hours of checkout with your order number.",
  },
  {
    id: "orders-3",
    category: "Orders",
    question: "Where can I see my order status?",
    answer:
      "Signed-in customers can track every order from Manage Account. Guest checkouts receive tracking in the confirmation and shipping emails.",
  },
  {
    id: "orders-4",
    category: "Orders",
    question: "Do you offer gift wrapping?",
    answer:
      "Yes. Choose gift wrap at checkout. We pack the item in tissue with a handwritten note and leave prices off the packing slip.",
  },
  {
    id: "payments-1",
    category: "Payments",
    question: "Which payment methods do you accept?",
    answer:
      "We accept Visa, Mastercard, American Express, Apple Pay, Google Pay, and Interno store credit. All payments are processed over encrypted checkout.",
  },
  {
    id: "payments-2",
    category: "Payments",
    question: "When will my card be charged?",
    answer:
      "Your card is authorized at checkout and captured when the order is packed. If an item is unavailable, we never charge for it.",
  },
  {
    id: "payments-3",
    category: "Payments",
    question: "Can I use more than one discount?",
    answer:
      "Coupons cannot be stacked with sale prices or welcome offers. Store credit can be combined with one promotional code.",
  },
  {
    id: "payments-4",
    category: "Payments",
    question: "Why was my payment declined?",
    answer:
      "Banks sometimes flag first-time fashion purchases. Try another card, confirm billing details match your bank record, or contact your issuer.",
  },
  {
    id: "account-1",
    category: "Account",
    question: "Do I need an account to shop?",
    answer:
      "No. Guest checkout is available. Creating an account lets you save addresses, track orders, and keep a wishlist across devices.",
  },
  {
    id: "account-2",
    category: "Account",
    question: "How do I reset my password?",
    answer:
      "Use Forgot password on the sign-in page. We send a reset link to the email on the account. Links expire after 30 minutes.",
  },
  {
    id: "account-3",
    category: "Account",
    question: "How do I update my details?",
    answer:
      "Open Manage Account to edit your name, phone, saved addresses, and marketing preferences. Email changes require a confirmation link.",
  },
  {
    id: "account-4",
    category: "Account",
    question: "How do I delete my account?",
    answer:
      "From Security settings you can request deletion. Open orders are completed first. We keep invoices as required by tax law.",
  },
  {
    id: "shipping-1",
    category: "Shipping",
    question: "How long does delivery take?",
    answer:
      "Standard arrives in 5-7 business days, Express in 2-3, and Next-day on the following business day if you order before 14:00.",
  },
  {
    id: "shipping-2",
    category: "Shipping",
    question: "Do you ship internationally?",
    answer:
      "Yes, to 28 countries. Duties and taxes are calculated at checkout so there are no surprise fees on delivery.",
  },
  {
    id: "shipping-3",
    category: "Shipping",
    question: "What if a package is missing?",
    answer:
      "Wait until the estimated delivery date, then contact us with photos of the packing slip. We open a carrier claim and send a replacement or refund.",
  },
  {
    id: "shipping-4",
    category: "Shipping",
    question: "Can I change the delivery address?",
    answer:
      "Yes, until the label is created. After that the carrier must redirect the parcel, which can add 1-2 days.",
  },
];

const CRUMBS: PageCrumb[] = [
  { label: "Home", to: "/Home" },
  { label: "FAQ" },
];

export default function FaqPage() {
  const [category, setCategory] = useState<FaqCategory>("Orders");
  const [query, setQuery] = useState("");
  const [openId, setOpenId] = useState<string | null>("orders-1");

  const visibleItems = useMemo(() => {
    const needle = query.trim().toLowerCase();
    return FAQ_ITEMS.filter((item) => {
      const inCategory = item.category === category;
      if (!needle) return inCategory;
      return (
        inCategory &&
        (item.question.toLowerCase().includes(needle) ||
          item.answer.toLowerCase().includes(needle))
      );
    });
  }, [category, query]);

  return (
    <PageLayout
      title="Frequently asked questions"
      subtitle="Quick answers about orders, payments, your account, and delivery. Search or browse by topic."
      crumbs={CRUMBS}
    >
      <section className="info-section">
        <div className="info-search">
          <Search size={16} className="info-search-icon" aria-hidden="true" />
          <input
            type="search"
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            placeholder="Search questions"
            aria-label="Search frequently asked questions"
          />
        </div>

        <div className="info-tabs" role="tablist" aria-label="FAQ categories">
          {CATEGORIES.map((item) => (
            <button
              key={item}
              type="button"
              role="tab"
              className={`info-tab ${category === item ? "active" : ""}`}
              aria-selected={category === item}
              onClick={() => {
                setCategory(item);
                const first = FAQ_ITEMS.find((faq) => faq.category === item);
                setOpenId(first?.id ?? null);
              }}
            >
              {item}
            </button>
          ))}
        </div>

        {visibleItems.length === 0 ? (
          <p className="empty-copy">
            No questions match that search in {category}. Try another keyword.
          </p>
        ) : (
          <div className="faq-list">
            {visibleItems.map((item) => {
              const isOpen = openId === item.id;
              return (
                <article className="faq-item" key={item.id}>
                  <h2>
                    <button
                      type="button"
                      className="faq-trigger"
                      aria-expanded={isOpen}
                      aria-controls={`${item.id}-panel`}
                      id={`${item.id}-button`}
                      onClick={() => setOpenId(isOpen ? null : item.id)}
                    >
                      <span>{item.question}</span>
                      <ChevronDown size={18} className="faq-chevron" />
                    </button>
                  </h2>
                  <div
                    id={`${item.id}-panel`}
                    role="region"
                    aria-labelledby={`${item.id}-button`}
                    className={`faq-panel ${isOpen ? "open" : ""}`}
                  >
                    <div className="faq-panel-inner">
                      <p>{item.answer}</p>
                    </div>
                  </div>
                </article>
              );
            })}
          </div>
        )}
      </section>

      <section className="info-card info-help-cta">
        <div>
          <h2>Still need help?</h2>
          <p className="info-lead">
            Our studio team replies on business days, usually within a few hours.
          </p>
        </div>
        <Link to="/Home/Contact" className="info-btn">
          Contact Interno
        </Link>
      </section>
    </PageLayout>
  );
}
