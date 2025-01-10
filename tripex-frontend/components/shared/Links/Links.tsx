import React from "react";
import Link from "next/link";
import { LINKS } from "../Links/links.data";

export const Links = () => {
  return (
    <div className="flex flex-col items-start justify-center min-h-screen bg-gray-100 px-8 ">
      <div className="mb-6 text-[50px] font-bold text-gray-800">Tripex</div>

      <ul className="space-y-4">
        {LINKS.map((link) => (
          <li key={link.id}>
            <Link
              href={link.href}
              className="text-lg font-semibold text-blue-600 hover:text-blue-800 transition-colors"
            >
              {link.name}
            </Link>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Links;
