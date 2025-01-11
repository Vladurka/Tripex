// components/Post.tsx

// import React from "react";
// import Image from "next/image";
// // import { Post as PostType } from "../data/post.data"; // Import your Post type

// // Define props with TypeScript
// interface PostProps {
//   post: PostType; // The 'post' prop will be of type 'Post'
// }

// export const Post = ({ post }: PostProps) => {
//   return (
//     <div className="border rounded-lg p-4 mb-6 bg-white shadow">
//       {/* Header */}
//       <div className="flex items-center justify-between">
//         <div className="flex items-center space-x-3">
//           <Image
//             src={post.avatar}
//             alt={post.name}
//             width={30}
//             height={30}
//             className="w-10 h-10 rounded-full"
//           />
//           <div>
//             <p className="font-semibold">{post.name}</p>
//             <p className="text-sm text-gray-500">{post.time}</p>
//           </div>
//         </div>
//         <div className="text-gray-500">•••</div>
//       </div>

//       {/* Content */}
//       <div className="mt-4">
//         <Image
//           src={post.content}
//           alt="Post content"
//           width={30}
//           height={30}
//           className="w-full rounded-lg"
//         />
//       </div>

//       {/* Actions */}
//       <div className="flex justify-between items-center mt-4">
//         <div className="flex space-x-4 text-gray-600">
//           <button>❤️ {post.likes}</button>
//           <button>💬 {post.comments}</button>
//         </div>
//         <button className="text-sm text-gray-500">Add comment</button>
//       </div>
//     </div>
//   );
// };

// export default Post;
