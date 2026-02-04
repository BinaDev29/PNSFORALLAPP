# Deployment Guide for PNS Dashboard

This guide explains how to deploy your React application to a shared hosting provider (like InfinityFree, Hostinger, etc.).

## 1. Create a Production Build
Before deploying, you must compile your code into static files.
Run this command in your terminal:

```bash
npm run build
```

This will create a **`dist`** folder in your project directory containing:
- `index.html`
- `assets/` (css and js files)
- `.htaccess` (for routing configuration)

## 2. Prepare Your Server
1. Log in to your hosting provider's **Control Panel** or **File Manager**.
2. Navigate to the public folder. This is usually named **`htdocs`** or **`public_html`**.
3. **Delete** any default files currently in that folder (e.g., `index2.html`, default welcome pages).

## 3. Upload Files
1. Open the **`dist`** folder on your computer.
2. Select **ALL** files inside `dist` (`index.html`, `.htaccess`, `assets`, etc.).
3. Upload these files directly into your server's `htdocs` folder.

> **Important:** Do NOT upload the `dist` folder itself. The `index.html` file must be directly inside `htdocs`.

## 4. Verify Deployment
- Open your website URL in a browser.
- Click around to different pages (e.g., Dashboard, History) to ensure the router is working.
- Refresh the page while on a sub-page (e.g., `/history`) to verify that the `.htaccess` file is correctly handling redirects (preventing 404 errors).

## Troubleshooting
- **White Screen**: Check the console (F12) for 404 errors on `.js` or `.css` files. Ensure you uploaded the `assets` folder correctly.
- **404 on Refresh**: If refreshing the page gives a 404 error, ensure the `.htaccess` file was uploaded. If you are on Nginx or another server type, you may need different configuration.
