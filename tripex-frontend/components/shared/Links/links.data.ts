export interface Links {
  id: number;
  name: string;
  href: string;
}

export const LINKS: Links[] = [
  {
    id: 1,
    name: "Home",
    href: "/",
  },
  {
    id: 2,
    name: "Create",
    href: "/create",
  },
  {
    id: 3,
    name: "Profile",
    href: "/profile",
  },
  {
    id: 4,
    name: "Notifications",
    href: "/notifications",
  },
];
