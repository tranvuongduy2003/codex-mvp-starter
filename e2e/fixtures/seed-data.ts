import { readFileSync } from "node:fs";
import { resolve } from "node:path";

const DATA_DIR = resolve(__dirname, "../../src/DataSeeder/Data");

export interface SeedUser {
  id: string;
  displayName: string;
  email: string;
  role: "Admin" | "User";
}

function loadJson<T>(filename: string): T {
  return JSON.parse(readFileSync(resolve(DATA_DIR, filename), "utf8")) as T;
}

export const users: SeedUser[] = loadJson<SeedUser[]>("Users.json");
export const SEED_PASSWORD = "DevPass123!";

export const admin = users.find((u) => u.role === "Admin") ?? users[0];
export const demoUser = users.find((u) => u.role === "User") ?? users[0];
