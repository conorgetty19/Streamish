import React from "react";
import { Card, CardBody } from "reactstrap";

const Video = ({ video }) => {
    return (
        <Card >
            <p className="text-left px-2">Posted by: {video.userProfile.name}</p>
            <CardBody>
                <iframe className="video"
                    src={video.url}
                    title="YouTube video player"
                    frameBorder="0"
                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                    allowFullScreen />

                <p>
                    <strong>{video.title}</strong>
                </p>
                <p>{video.description}</p>

                {video.comments.length > 0 ?
                    <div>
                        <h5>Comments:</h5>
                        {video.comments?.map((comment) => (
                            <div key={comment.id}>
                                <p>{comment.message}</p>
                                <p>Posted by: {comment.userProfile.name}</p>
                            </div>))}
                    </div>
                    : null}


            </CardBody>
        </Card>
    );
};

export default Video;