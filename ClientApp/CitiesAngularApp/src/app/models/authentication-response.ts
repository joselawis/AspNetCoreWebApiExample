export class AuthenticationResponse {
  personName: string | null = null;
  email: string | null = null;
  token: string | null = null;
  expiration: Date | null = null;
}
