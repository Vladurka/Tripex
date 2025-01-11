export interface Links {
  id: number;
  src: string;
  alt: string;
  text: string;
  name: string;
  href: string;
}

export const LINKS: Links[] = [
  {
    id: 1,
    src: "/Links/home-icon.svg",
    alt: "Home",
    text: "Home",
    name: "Home",
    href: "/",
  },
  {
    id: 2,
    src: "/Links/search-icon.svg",
    alt: "Search",
    text: "Search",
    name: "Search",
    href: "/search",
  },
  {
    id: 3,
    src: "/Links/create-icon.svg",
    alt: "Create",
    text: "Create",
    name: "Create",
    href: "/create",
  },

  {
    id: 4,
    src: "/Links/notifications-icon.svg",
    alt: "Notifications",
    text: "Notifications",
    name: "Notifications",
    href: "/notifications",
  },
  {
    id: 5,
    src: "/Links/profile-icon.svg",
    alt: "Profile",
    text: "Profile",
    name: "Profile",
    href: "/profile",
  },
];
