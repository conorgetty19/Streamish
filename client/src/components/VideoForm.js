import React, { useState } from "react";

const VideoForm = ({ addVideo, getVideos }) => {
    //variable to hold state and a setter function
    //allows us to keep track of data and change the values of said data
    const [title, setTitle] = useState("");
    const [url, setUrl] = useState("");
    const [description, setDescription] = useState("");

    const handleSubmit = (e) => {
        e.preventDefault();

        // Create a new video object with the entered data
        //so that we can send it to the database as part of a post fetch call
        const newVideo = {
            title: title,
            url: url,
            description: description,
        };

        addVideo(newVideo).then(() => {
            // Call getVideos function (passed as prop) to fetch the updated list
            getVideos();

            // Reset the form fields
            setTitle("");
            setUrl("");
            setDescription("");
        });
    };

    return (
        <div>
            <h3>Add New Video</h3>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Title:</label>
                    <input
                        type="text"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>URL:</label>
                    <input
                        type="text"
                        value={url}
                        onChange={(e) => setUrl(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>Description:</label>
                    <textarea
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                    />
                </div>
                <button type="submit">Add Video</button>
            </form>
        </div>
    );
};

export default VideoForm;
