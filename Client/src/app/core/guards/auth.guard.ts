import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthToken } from '../interfaces/api.interface';
import { TokenService } from '../services/token.service';
import { LanguageService } from '../services/language.service';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const tokenService = inject(TokenService);
  const lang = inject(LanguageService).lang();

  const token = tokenService.getToken();
  if (token) {
    const tokenData: AuthToken = token;
    if (tokenData.accessTokenExpires) {
      const expirationDate = new Date(tokenData.accessTokenExpires);
      if (expirationDate > new Date()) {
        return true; // Token is valid
      } else {
        tokenService.removeToken();
        return router.createUrlTree([lang, 'login']); // Token has expired
      }
    } else {
      return router.createUrlTree([lang, 'login']); // No expiration time found
    }
  } else {
    return router.createUrlTree([lang, 'login']); // No token found
  }
};

// notAuthGuard to prevent authenticated users from accessing login and register pages
export const notAuthGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const tokenService = inject(TokenService);
  const lang = inject(LanguageService).lang();

  const token = tokenService.getToken();
  if (token) {
    const tokenData: AuthToken = token;
    if (tokenData.accessTokenExpires) {
      const accessTokenExpires = new Date(tokenData.accessTokenExpires);
      if (accessTokenExpires > new Date()) {
        return router.createUrlTree([lang]); // Redirect to home if authenticated
      } else {
        tokenService.removeToken();
        return router.createUrlTree([lang, 'login']); // Token expired
      }
    }
  }
  return true; // User is not authenticated, allow access
};

export const adminGuard: CanActivateFn = () => {
  const router = inject(Router);
  const tokenService = inject(TokenService);
  const lang = inject(LanguageService).lang();

  const token = tokenService.getToken();
  const rawUser = localStorage.getItem('user');
  const user = rawUser ? JSON.parse(rawUser) : null;

  if (!token || !user?.roles) {
    return router.createUrlTree([lang, 'dashboard']);
  }

  const roles = Array.isArray(user.roles) ? user.roles : String(user.roles).split(',');
  const isAdmin = roles.some((role: string) => role.trim().toLowerCase() === 'admin');

  return isAdmin ? true : router.createUrlTree([lang, 'dashboard']);
};
