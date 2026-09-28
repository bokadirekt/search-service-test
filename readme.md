# Booking service

## Description
Build a small service that lets a consumer **find** a bookable service near them and **book** a time.

It should support two operations:

1. **Search** – given a service name, a location and a date range, return matching services together with available time slots.
2. **Book** – given a slot from the search result, create a booking.

The service can communicate over HTTP or standard input/output. Language, framework and storage are up to you.

**It must run straight after cloning**
Only the language runtime and its standard package install (e.g. npm install, pip install -r requirements.txt) may be required. No Docker, databases or other external services. In-memory or embedded storage (a file, SQLite) is fine.

## How we want you to work
We expect you to use AI tools (Claude Code, Cursor, Copilot, ChatGPT, whatever you normally use) as much as you like. That is how we work every day.

What we are interested in is *your* judgment: how you frame the problem, which decisions you make, how you verify what the AI produces, and what you choose not to build.

The spec below is intentionally incomplete. Where something is unclear, make a decision, write it down and move on. There is no hidden "correct" answer.

**Timebox: max 2 hours.** Tell us what you cut and why.

## Data
Read from `data.json` in the root of this repository. It contains venues, services, staff and existing bookings. Treat it as real production data.

## Input
**Search**
* Service name (free text)
* Geolocation (lat/lng)
* Date range

**Book**
* A slot returned by search
* Customer name

## Output
JSON. Search results should at least include the venue, the service, the distance and available slots. Design the rest of the format yourself.

Booking should return the created booking, or a clear error.

## Deliverables
1. **Code** in a git repo (share a link or zip). Keep the commit history.
2. **Tests** for the parts you consider most important.
3. **`DECISIONS.md`** (max 1 page)
   * Assumptions you made and why
   * Trade-offs, and what you would change at 100x the data or traffic
   * What you cut
4. **`AI.md`**
   * Which tools you used and for what
   * 2–3 places where you corrected, rejected or overrode the AI's output, and why
