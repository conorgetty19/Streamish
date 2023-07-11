import React, { useEffect, useState } from "react";
import Video from './Video';
import VideoForm from "../components/VideoForm";
import { getAllVideos, getSearchedVideos, addVideo } from "../modules/videoManager";

const VideoList = () => {
    const [videos, setVideos] = useState([]);
    const [searchTerms, setSearchTerm] = useState("")


    const getVideosBySearch = () => {
        getSearchedVideos(searchTerms).then(videos => setVideos(videos));
    };

    const getVideos = () => {
        getAllVideos().then(videos => setVideos(videos));
    }

    useEffect(() => {
        getVideos();
    }, []);

    return (
        <>
            <VideoForm addVideo={addVideo} />
            <div className="container">
                <input type="text" placeholder="Search terms" id="search-input" value={searchTerms} onChange={(e) => (setSearchTerm(e.target.value))}></input>
                <button type="button" onClick={getVideosBySearch}>Search Videos</button>
                <button type="button" onClick={getVideos}>All Videos</button>
                <div className="row justify-content-center">
                    {videos.map((video) => (
                        <Video video={video} key={video.id} />
                    ))}
                </div>
            </div>
        </>
    );
};

export default VideoList;