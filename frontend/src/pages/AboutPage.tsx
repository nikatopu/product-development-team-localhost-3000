import styles from './AboutPage.module.css';

export function AboutPage() {
  return (
    <div className={styles.page}>
      <header className={styles.pageHeader}>
        <span className={styles.label}>// about</span>
        <h1 className={styles.title}>What is Driftless?</h1>
        <p className={styles.subtitle}>
          An API documentation tool that reads your source code so your docs never fall behind.
        </p>
      </header>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>The Problem</h2>
        <p>
          Backend developers change code constantly — fields get renamed, types shift, new
          endpoints appear. Frontend developers and API consumers are left guessing. By the
          time anyone notices, the documentation is weeks out of date.
        </p>
        <p>
          This gap between a living codebase and stale docs is <strong>API drift</strong>.
          It costs time, causes bugs, and breaks trust between teams.
        </p>
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>How Driftless Works</h2>
        <div className={styles.steps}>
          <div className={styles.step}>
            <span className={styles.stepNum}>01</span>
            <div>
              <h3>Clone</h3>
              <p>Driftless clones the repository directly from GitHub using LibGit2Sharp — no manual download needed.</p>
            </div>
          </div>
          <div className={styles.step}>
            <span className={styles.stepNum}>02</span>
            <div>
              <h3>Parse</h3>
              <p>Microsoft's Roslyn compiler reads the C# source code and builds a full syntax tree of every class and method.</p>
            </div>
          </div>
          <div className={styles.step}>
            <span className={styles.stepNum}>03</span>
            <div>
              <h3>Extract</h3>
              <p>Every controller, route attribute, parameter, request body DTO, and response type is pulled out of the syntax tree.</p>
            </div>
          </div>
          <div className={styles.step}>
            <span className={styles.stepNum}>04</span>
            <div>
              <h3>Generate</h3>
              <p>Routes are rendered in a browsable UI. TypeScript interface definitions and fetch functions are generated from the extracted types.</p>
            </div>
          </div>
        </div>
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>What's Supported</h2>
        <div className={styles.twoCol}>
          <div>
            <h3 className={styles.colHead}>Supported</h3>
            <ul className={styles.list}>
              <li><span className={styles.ok}>✓</span> ASP.NET Core Web API (attribute routing)</li>
              <li><span className={styles.ok}>✓</span> .NET 6, 8, and 10</li>
              <li><span className={styles.ok}>✓</span> Public GitHub repositories</li>
              <li><span className={styles.ok}>✓</span> C# controller classes</li>
              <li><span className={styles.ok}>✓</span> Nested DTO properties</li>
              <li><span className={styles.ok}>✓</span> Route, query, and body parameters</li>
            </ul>
          </div>
          <div>
            <h3 className={styles.colHead}>Not Yet Supported</h3>
            <ul className={styles.list}>
              <li><span className={styles.na}>–</span> Minimal APIs</li>
              <li><span className={styles.na}>–</span> Private repositories</li>
              <li><span className={styles.na}>–</span> Java, Go, Python backends</li>
              <li><span className={styles.na}>–</span> Authentication / auth headers</li>
              <li><span className={styles.na}>–</span> Request example values</li>
            </ul>
          </div>
        </div>
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>Tech Stack</h2>
        <div className={styles.stack}>
          <div className={styles.stackGroup}>
            <span className={styles.stackLabel}>Backend</span>
            <div className={styles.stackItems}>
              <span className={styles.chip}>ASP.NET Core 10</span>
              <span className={styles.chip}>Roslyn</span>
              <span className={styles.chip}>LibGit2Sharp</span>
              <span className={styles.chip}>Swashbuckle</span>
            </div>
          </div>
          <div className={styles.stackGroup}>
            <span className={styles.stackLabel}>Frontend</span>
            <div className={styles.stackItems}>
              <span className={styles.chip}>React 19</span>
              <span className={styles.chip}>TypeScript</span>
              <span className={styles.chip}>Vite</span>
            </div>
          </div>
        </div>
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>Built By</h2>
        <p className={styles.teamLine}>
          KIU Product Development Team &mdash; built as part of a computer science product
          development course. The goal: ship a real, useful tool from scratch.
        </p>
        <div className={styles.team}>
          <div className={styles.member}>
            <span className={styles.role}>Tech Lead</span>
            <span className={styles.name}>Nikoloz Topuridze</span>
          </div>
          <div className={styles.member}>
            <span className={styles.role}>Discovery Lead</span>
            <span className={styles.name}>Toma Danelia</span>
          </div>
          <div className={styles.member}>
            <span className={styles.role}>Program Lead</span>
            <span className={styles.name}>Giorgi Tkebuchava</span>
          </div>
        </div>
      </section>
    </div>
  );
}
