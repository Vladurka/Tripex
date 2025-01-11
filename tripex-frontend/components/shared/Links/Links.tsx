import Link from "next/link";
import { LINKS } from "../Links/links.data";
import Image from "next/image";

export const Links = () => {
  return (
    <div className="flex flex-col items-start max-w-[350px] justify-start min-h-screen bg-background text-foreground px-8 py-4 border-r border-gray-500">
      <div className="mb-6 text-[40px] text-[#8c52ff] font-bold">tripex</div>

      <ul className="space-y-7 ">
        {LINKS.map((link) => (
          <li key={link.id} className="flex items-center space-x-4 mb-4">
            <Image src={link.src} alt={link.alt} width={30} height={30} />
            <Link
              href={link.href}
              className="text-lg font-semibold hover:text-gray-400 transition-colors"
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
