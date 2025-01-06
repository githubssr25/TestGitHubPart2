import { useEffect, useState } from "react";
import { getAllRepositories } from "./manager/repositoryManager";
// import "./AllRepositories.css";

export const SearchRepositories = ({loggedInUser, setLoggedInUser}) => {
  const [repositories, setRepositories] = useState([]);

  console.log("what is loggedinUser", loggedInUser);

//   useEffect(() => {
//     getAllRepositories().then(setRepositories).catch(console.error);
//   }, []);

useEffect(() => {
    getAllRepositories()
      .then((data) => {
        console.log("Fetched repositories:", data); // Log the response data
        setRepositories(data); // Set the repositories state with the data
      })
      .catch((error) => {
        console.error("Error fetching repositories:", error); // Log any errors
      });
  }, []);
  

  return (
    <div className="repositories-container">
      <h2>All Repositories</h2>
      <ul className="repositories-list">
        {repositories.map((repo, index) => (
          <li key={index}>
            <a href={repo.repositoryUrl} target="_blank" rel="noopener noreferrer">
              {repo.repositoryName}
            </a>
            <p>{repo.description}</p>
            <p>{repo.language} - {repo.stars} Stars</p>
            {repo.category && <p>Category: {repo.category.description}</p>}
          </li>
        ))}
      </ul>
    </div>
  );
};
