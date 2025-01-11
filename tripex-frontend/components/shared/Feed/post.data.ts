export interface Post {
  id: number;
  name: string;
  avatar: string;
  time: string;
  content: string;
  likes: number;
  comments: number;
}

export const POSTS_DATA: Post[] = [
  {
    id: 1,
    name: "Kamrik",
    avatar: "/Avatar/images.png",
    time: "2 hours ago",
    content: "/Avatar/images.png",
    likes: 120,
    comments: 45,
  },
  {
    id: 2,
    name: "Vladik",
    avatar: "/Avatar/images.png",
    time: "5 hours ago",
    content: "/Avatar/images.png",
    likes: 200,
    comments: 30,
  },
];
