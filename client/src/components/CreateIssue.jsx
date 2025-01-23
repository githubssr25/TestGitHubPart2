import { useState } from "react";
import { createIssue} from "./manager/issueManager";
import { useNavigate } from "react-router-dom";

export const CreateIssue = () => {
    const navigate = useNavigate();
    const [title, setTitle] = useState("");
    const [body, setBody] = useState("");
    const [repositoryOwner, setRepositoryOwner] = useState("");
    const [repositoryName, setRepositoryName] = useState("");
    const [labels, setLabels] = useState("");
    const [error, setError] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [isRepoValid, setIsRepoValid] = useState(false);
    const [createdIssueUrl, setCreatedIssueUrl] = useState(null);
    
  
    // New fields
    const [codePath, setCodePath] = useState("");
    const [codeSnippet, setCodeSnippet] = useState("");
    const [stepsToReproduce, setStepsToReproduce] = useState("");
    const [expectedBehavior, setExpectedBehavior] = useState("");
    const [actualBehavior, setActualBehavior] = useState("");
    const [environment, setEnvironment] = useState("");

    const [repoUrl, setRepoUrl] = useState("");

const verifyRepository = async () => {
  try {
    // Parse owner and name from URL (e.g., https://github.com/owner/repo)
    const urlParts = repoUrl.split('/');
    const owner = urlParts[urlParts.length - 2];
    const name = urlParts[urlParts.length - 1];

    const verifyUrl = `https://api.github.com/repos/${owner}/${name}`;
    const response = await fetch(verifyUrl);
    
    if (response.ok) {
      setRepositoryOwner(owner);
      setRepositoryName(name);
      setIsRepoValid(true);
      setError("");
    } else {
      setError("Invalid repository URL or no access");
      setIsRepoValid(false);
    }
  } catch {
    setError("Invalid repository URL format");
    setIsRepoValid(false);
  }
};
  
const handleSubmit = async (e) => {
  e.preventDefault();
  setIsSubmitting(true);
  setError("");
 
  try {
    const labelsArray = labels
      .split(",")
      .map((label) => label.trim())
      .filter((label) => label.length > 0);
 
    const response = await createIssue({
      title,
      body,
      repositoryOwner,
      repositoryName,
      repositoryUrl: repoUrl,
      labels: labelsArray,
      codePath,
      codeSnippet, 
      stepsToReproduce,
      expectedBehavior,
      actualBehavior,
      environment
    });
 
    // Response now includes both GitHub issue and local copy
    const { gitHubIssue, localIssue } = response;
    
    setCreatedIssueUrl(gitHubIssue.html_url);
    window.open(gitHubIssue.html_url, 'github-issue');
    
    navigate(`/issues/${localIssue.id}`);
  } catch (err) {
    setError(err.message);
  } finally {
    setIsSubmitting(false);
  }
 };
    return (
      <div className="create-issue-container">
        <h2>Create New Issue</h2>
        {error && <div className="error-message">{error}</div>}
        {createdIssueUrl && (
          <div className="success-message">
            Issue created successfully!{" "}
            <a href={createdIssueUrl} target="_blank" rel="noopener noreferrer">
              View on GitHub
            </a>
          </div>
        )}

        <form onSubmit={handleSubmit} className="create-issue-form">
          <div className="form-group">
            <label>Repository Owner:</label>
            <input
              type="text"
              value={repositoryOwner}
              onChange={(e) => setRepositoryOwner(e.target.value)}
              onBlur={verifyRepository}
              required
              placeholder="e.g., microsoft"
            />
          </div>

          <div className="form-group">
            <label>Repository URL:</label>
            <input
              type="url"
              value={repoUrl}
              onChange={(e) => setRepoUrl(e.target.value)}
              onBlur={verifyRepository}
              required
              placeholder="https://github.com/owner/repository"
            />
          </div>

          <div className="form-group">
            <label>Repository Name:</label>
            <input
              type="text"
              value={repositoryName}
              onChange={(e) => setRepositoryName(e.target.value)}
              onBlur={verifyRepository}
              required
              placeholder="e.g., vscode"
            />
          </div>

          <div className="form-group">
            <label>Title:</label>
            <input
              type="text"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              required
              placeholder="Issue title"
            />
          </div>

          <div className="form-group">
            <label>Description:</label>
            <textarea
              value={body}
              onChange={(e) => setBody(e.target.value)}
              required
              placeholder="Describe the issue..."
              rows="5"
            />
          </div>

          <div className="form-group">
            <label>Code Path:</label>
            <input
              type="text"
              value={codePath}
              onChange={(e) => setCodePath(e.target.value)}
              placeholder="Path to the affected code"
            />
          </div>

          <div className="form-group">
            <label>Code Snippet:</label>
            <textarea
              value={codeSnippet}
              onChange={(e) => setCodeSnippet(e.target.value)}
              placeholder="Relevant code snippet"
            />
          </div>

          <div className="form-group">
            <label>Steps to Reproduce:</label>
            <textarea
              value={stepsToReproduce}
              onChange={(e) => setStepsToReproduce(e.target.value)}
              placeholder="Describe how to reproduce the issue"
            />
          </div>

          <div className="form-group">
            <label>Expected Behavior:</label>
            <input
              type="text"
              value={expectedBehavior}
              onChange={(e) => setExpectedBehavior(e.target.value)}
              placeholder="What should happen?"
            />
          </div>

          <div className="form-group">
            <label>Actual Behavior:</label>
            <input
              type="text"
              value={actualBehavior}
              onChange={(e) => setActualBehavior(e.target.value)}
              placeholder="What actually happened?"
            />
          </div>

          <div className="form-group">
            <label>Environment:</label>
            <input
              type="text"
              value={environment}
              onChange={(e) => setEnvironment(e.target.value)}
              placeholder="OS, Browser, Version"
            />
          </div>

          <button
            type="submit"
            disabled={isSubmitting}
            className="submit-button"
          >
            {isSubmitting ? "Creating..." : "Create Issue"}
          </button>
        </form>
      </div>
    );
  };