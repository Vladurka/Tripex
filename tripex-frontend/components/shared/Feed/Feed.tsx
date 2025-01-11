// components/Feed.tsx
import React from "react";
import { POSTS_DATA } from "./post.data";
import Post from "./Post";

const Feed = () => {
  return (
    <div>
      {POSTS_DATA.map((post) => (
        <Post key={post.id} post={post} />
      ))}
    </div>
  );
};

export default Feed;
