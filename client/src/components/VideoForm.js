import React, { useState } from "react";

const VideoForm = ({ addVideo }) => {
    const [title, setTitle] = useState("");
    const [url, setUrl] = useState("");
    const [description, setDescription] = useState("");

    const handleSubmit = (e) => {
        e.preventDefault();

        // Create a new video object with the entered data
        const newVideo = {
            title: title,
            url: url,
            description: description,
        };

        // Call the addVideo function passed as a prop to add the new video
        addVideo(newVideo);

        // Reset the form fields
        setTitle("");
        setUrl("");
        setDescription("");
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
