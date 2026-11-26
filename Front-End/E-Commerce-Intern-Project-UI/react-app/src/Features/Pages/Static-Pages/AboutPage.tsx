import { Link } from "react-router-dom";
import { Leaf, Shield, Sparkles } from "lucide-react";
import PageLayout, { PageCrumb } from "./PageLayout";

type StatItem = {
  value: string;
  label: string;
};

type ValueItem = {
  title: string;
  body: string;
  icon: "quality" | "sustainability" | "service";
};

type TeamMember = {
  name: string;
  role: string;
  initials: string;
};

const STORY = [
  "Interno began as a small Cairo studio making considered clothes for everyday life: clean silhouettes, honest fabrics, and pieces you actually wear twice in a week.",
  "We still design in-house, sample on the workroom table, and keep production close enough that we can visit the floor. The goal is not a louder wardrobe. It is a better one.",
];

const STATS: StatItem[] = [
  { value: "48k+", label: "Customers" },
  { value: "320", label: "Products" },
  { value: "28", label: "Countries" },
  { value: "7", label: "Years" },
];

const VALUES: ValueItem[] = [
  {
    icon: "quality",
    title: "Quality",
    body: "We cut from fabrics that hold their shape after washing and finish every seam as if someone will look inside the garment.",
  },
  {
    icon: "sustainability",
    title: "Sustainability",
    body: "Deadstock programmes, recycled mailers, and small runs so we make what people actually buy.",
  },
  {
    icon: "service",
    title: "Service",
    body: "Real humans answer fit questions. If a piece fails, we repair, replace, or refund without a script.",
  },
];

const TEAM: TeamMember[] = [
  { name: "Lina Farouk", role: "Creative Director", initials: "LF" },
  { name: "Omar Hassan", role: "Head of Product", initials: "OH" },
  { name: "Maya Adel", role: "Studio Lead", initials: "MA" },
  { name: "Karim Nabil", role: "Client Services", initials: "KN" },
];

const ICONS = {
  quality: Sparkles,
  sustainability: Leaf,
  service: Shield,
};

const CRUMBS: PageCrumb[] = [
  { label: "Home", to: "/Home" },
  { label: "About" },
];

export default function AboutPage() {
  return (
    <PageLayout
      title="About Interno"
      subtitle="A clothing brand built around considered pieces, quiet luxury, and clothes that earn their place."
      crumbs={CRUMBS}
    >
      <section className="info-section">
        {STORY.map((paragraph) => (
          <p className="about-story" key={paragraph}>
            {paragraph}
          </p>
        ))}
      </section>

      <section className="info-section">
        <div className="stats-row">
          {STATS.map((stat) => (
            <article className="info-card stat-card" key={stat.label}>
              <strong>{stat.value}</strong>
              <h3>{stat.label}</h3>
            </article>
          ))}
        </div>
      </section>

      <section className="info-section">
        <h2>What we stand for</h2>
        <div className="info-grid cols-3">
          {VALUES.map((value) => {
            const Icon = ICONS[value.icon];
            return (
              <article className="info-card" key={value.title}>
                <span className="info-icon">
                  <Icon size={18} aria-hidden="true" />
                </span>
                <h3>{value.title}</h3>
                <p>{value.body}</p>
              </article>
            );
          })}
        </div>
      </section>

      <section className="info-section">
        <h2>The studio team</h2>
        <div className="info-grid cols-4">
          {TEAM.map((member) => (
            <article className="info-card team-card" key={member.name}>
              <div className="team-avatar" aria-hidden="true">
                {member.initials}
              </div>
              <h3>{member.name}</h3>
              <p>{member.role}</p>
            </article>
          ))}
        </div>
      </section>

      <section className="info-card info-help-cta">
        <div>
          <h2>See the current collection</h2>
          <p className="info-lead">
            New drops sit alongside the pieces we never take off the floor.
          </p>
        </div>
        <Link
          to="/Home/Products?Category=All&SubCategory=All&Price=0&Size=All&Color=All"
          className="info-btn"
        >
          Shop Interno
        </Link>
      </section>
    </PageLayout>
  );
}
