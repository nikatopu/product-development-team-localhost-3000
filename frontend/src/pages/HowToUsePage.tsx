import styles from './HowToUsePage.module.css';

export function HowToUsePage() {
  return (
    <div className={styles.page}>
      <header className={styles.pageHeader}>
        <span className={styles.label}>// how-to-use</span>
        <h1 className={styles.title}>Getting Started</h1>
        <p className={styles.subtitle}>
          From URL to API docs in under 30 seconds.
        </p>
      </header>

      <div className={styles.steps}>

        {/* Step 1 */}
        <div className={styles.step}>
          <div className={styles.stepAside}>
            <span className={styles.stepNum}>01</span>
          </div>
          <div className={styles.stepBody}>
            <h2 className={styles.stepTitle}>Find a repository</h2>
            <p>
              You need a public GitHub repository that contains an ASP.NET Core Web API project.
              Copy the HTTPS clone URL or the repository's browser URL.
            </p>
            <div className={styles.codeBlock}>
              <span className={styles.comment}>// Example URL</span>
              <code>https://github.com/tomadanelia/community-dashboard.git</code>
            </div>
            <div className={styles.note}>
              <span className={styles.noteTag}>NOTE</span>
              Only public repositories are supported. Private repos require authentication which is not yet implemented.
            </div>
          </div>
        </div>

        {/* Step 2 */}
        <div className={styles.step}>
          <div className={styles.stepAside}>
            <span className={styles.stepNum}>02</span>
          </div>
          <div className={styles.stepBody}>
            <h2 className={styles.stepTitle}>Select a branch</h2>
            <p>
              The analyzer defaults to the <code className={styles.inlineCode}>main</code> branch.
              If your API lives on a different branch, click the branch button next to the URL
              field and type the branch name.
            </p>
            <div className={styles.codeBlock}>
              <span className={styles.comment}>// Examples</span>
              <code>main</code>
              <code>master</code>
              <code>develop</code>
              <code>feature/new-endpoints</code>
            </div>
          </div>
        </div>

        {/* Step 3 */}
        <div className={styles.step}>
          <div className={styles.stepAside}>
            <span className={styles.stepNum}>03</span>
          </div>
          <div className={styles.stepBody}>
            <h2 className={styles.stepTitle}>Click Analyze</h2>
            <p>
              Hit the <strong>Analyze</strong> button. Driftless will clone the repository,
              scan for <code className={styles.inlineCode}>.csproj</code> files, locate all
              controllers, and extract every route. This typically takes 5–20 seconds depending
              on repository size.
            </p>
            <div className={styles.processList}>
              <div className={styles.processItem}>
                <span className={styles.arrow}>&rarr;</span>
                <span>Cloning repository</span>
              </div>
              <div className={styles.processItem}>
                <span className={styles.arrow}>&rarr;</span>
                <span>Locating .csproj and controller files</span>
              </div>
              <div className={styles.processItem}>
                <span className={styles.arrow}>&rarr;</span>
                <span>Parsing C# syntax with Roslyn</span>
              </div>
              <div className={styles.processItem}>
                <span className={styles.arrow}>&rarr;</span>
                <span>Extracting routes, parameters, and DTOs</span>
              </div>
              <div className={styles.processItem}>
                <span className={styles.arrow}>&rarr;</span>
                <span>Generating TypeScript types</span>
              </div>
            </div>
          </div>
        </div>

        {/* Step 4 */}
        <div className={styles.step}>
          <div className={styles.stepAside}>
            <span className={styles.stepNum}>04</span>
          </div>
          <div className={styles.stepBody}>
            <h2 className={styles.stepTitle}>Browse routes</h2>
            <p>
              The <strong>Routes</strong> tab shows every discovered endpoint. Each card displays
              the HTTP method, path, and a summary. Click any card to expand it and see full details.
            </p>
            <div className={styles.methodGrid}>
              <div className={styles.methodRow}>
                <span className={`${styles.badge} ${styles.get}`}>GET</span>
                <span>Read-only data retrieval</span>
              </div>
              <div className={styles.methodRow}>
                <span className={`${styles.badge} ${styles.post}`}>POST</span>
                <span>Create a new resource</span>
              </div>
              <div className={styles.methodRow}>
                <span className={`${styles.badge} ${styles.put}`}>PUT</span>
                <span>Replace a resource entirely</span>
              </div>
              <div className={styles.methodRow}>
                <span className={`${styles.badge} ${styles.patch}`}>PATCH</span>
                <span>Partial update of a resource</span>
              </div>
              <div className={styles.methodRow}>
                <span className={`${styles.badge} ${styles.delete}`}>DELETE</span>
                <span>Remove a resource</span>
              </div>
            </div>
            <p>
              Use the search box to filter by path or controller name. Use the method buttons
              to show only a specific HTTP verb.
            </p>
          </div>
        </div>

        {/* Step 5 */}
        <div className={styles.step}>
          <div className={styles.stepAside}>
            <span className={styles.stepNum}>05</span>
          </div>
          <div className={styles.stepBody}>
            <h2 className={styles.stepTitle}>Copy TypeScript types</h2>
            <p>
              The <strong>TypeScript</strong> tab generates two things from the extracted API contract:
            </p>
            <div className={styles.tsList}>
              <div className={styles.tsItem}>
                <span className={styles.tsTag}>interface</span>
                <span>TypeScript interface definitions for every DTO and response type</span>
              </div>
              <div className={styles.tsItem}>
                <span className={styles.tsTag}>fetch()</span>
                <span>Typed fetch functions that call each endpoint with the correct parameters</span>
              </div>
            </div>
            <p>
              Click <strong>Copy</strong> next to any block to grab it individually, or use
              the copy icon at the top of each section to get everything at once.
              Paste directly into your frontend project.
            </p>
            <div className={styles.codeBlock}>
              <span className={styles.comment}>// Generated example</span>
              <code><span className={styles.kw}>interface</span> <span className={styles.type}>UserDto</span> {'{'}</code>
              <code>{'  '}<span className={styles.prop}>id</span>: <span className={styles.type}>number</span>;</code>
              <code>{'  '}<span className={styles.prop}>name</span>: <span className={styles.type}>string</span>;</code>
              <code>{'  '}<span className={styles.prop}>email</span>: <span className={styles.type}>string</span>;</code>
              <code>{'}'}</code>
            </div>
          </div>
        </div>

      </div>

      <section className={styles.tips}>
        <h2 className={styles.tipsTitle}>Tips &amp; Gotchas</h2>
        <ul className={styles.tipsList}>
          <li>Only public GitHub repos are supported — private repos will fail to clone</li>
          <li>The repo must contain at least one <code className={styles.inlineCode}>.csproj</code> file</li>
          <li>Controller methods without explicit route attributes may not be detected</li>
          <li>Large repositories with many controllers may take longer — be patient</li>
          <li>If an analysis fails, check that the URL is correct and the repo is public</li>
        </ul>
      </section>
    </div>
  );
}
