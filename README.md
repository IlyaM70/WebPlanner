<h1>🗓️ WebPlanner</h1>
<p>A task management web application built with ASP.NET MVC, allowing users to create, edit, delete, and organize tasks with priorities, deadlines, descriptions, and hashtags.</p>

<h2>🚀 Features</h2>
<ul>
  <li>Create, edit, and delete tasks</li>
  <li>Assign priorities, deadlines, descriptions, and hashtags to tasks</li>
  <li>Search and filter tasks</li>
  <li>Sort tasks by various criteria</li>
</ul>

<h2>🛠️ Technologies Used</h2>
<ul>
  <li>Backend: ASP.NET MVC</li>
  <li>Frontend: HTML, CSS, Bootstrap, JavaScript, jQuery, Tagify</li>
  <li>Database: PostgreSQL</li>
  <li>ORM: Entity Framework</li>
  <li>Containerization: Docker</li>
</ul>

<h2>📥 Installation</h2>
<ol>
  <li><strong>Clone the repository</strong>
    <pre><code>git clone https://github.com/IlyaM70/WebPlanner.git
cd WebPlanner</code></pre>
  </li>
  <li><strong>Open the solution in Visual Studio</strong>
    <ul>
      <li>Launch Visual Studio.</li>
      <li>Open the <code>WebPlanner.sln</code> file.</li>
    </ul>
  </li>
  <li><strong>Configure the database</strong>
    <ul>
      <li>Ensure PostgreSQL is installed and running.</li>
      <li>Update the connection string in <code>appsettings.json</code> to match your PostgreSQL configuration.</li>
      <li>Apply migrations to set up the database schema.</li>
    </ul>
  </li>
  <li><strong>Build and run the application</strong>
    <ul>
      <li>Build the solution to restore any necessary packages.</li>
      <li>Run the application using the debugger or press <code>F5</code>.</li>
    </ul>
  </li>
</ol>

<h2>💻 Usage</h2>
<p>After launching the application:</p>
<ul>
  <li>Access the task management interface through your web browser.</li>
  <li>Create new tasks by providing necessary details.</li>
  <li>Edit or delete existing tasks as needed.</li>
  <li>Use the search and filter functionalities to organize tasks.</li>
</ul>

<h2>📂 Project Structure</h2>
<pre><code>WebPlanner/
├── WebPlanner/                 # Main application directory
│   ├── Controllers/            # MVC controllers
│   ├── Models/                 # Data models
│   ├── Views/                  # MVC views
│   └── wwwroot/                # Static files (CSS, JS, images)
├── WebPlanner.sln              # Visual Studio solution file
├── appsettings.json            # Application configuration
├── Dockerfile                  # Docker configuration
├── docker-compose.yml          # Docker Compose configuration
└── README.md                   # Project documentation</code></pre>
