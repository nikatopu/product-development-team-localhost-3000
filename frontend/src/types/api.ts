export interface PropertyInfo {
  name: string;
  type: string;
  isRequired: boolean;
  summary?: string;
  nestedProperties?: PropertyInfo[];
}

export interface Parameter {
  name: string;
  type: string;
  source: string;
  isRequired: boolean;
}

export interface RequestBody {
  typeName: string;
  properties: PropertyInfo[];
}

export interface RouteResponse {
  statusCode: number;
  description: string;
  typeName: string;
  properties: PropertyInfo[];
}

export interface Route {
  method: 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH';
  path: string;
  controller: string;
  action: string;
  summary?: string;
  attributes: string[];
  parameters: Parameter[];
  requestBody?: RequestBody;
  responses: RouteResponse[];
}

export interface BreakingChange {
  changeType: string;
  severity: 'Breaking' | 'NonBreaking';
  affectedEndpoint: string;
  affectedField?: string;
  oldValue?: string;
  newValue?: string;
}

export interface RoutesJsonResult {
  projectName: string;
  analyzedAt: string;
  metadata: {
    totalRoutes: number;
    totalControllers: number;
    apiType: string;
    detectedFrameworks: string[];
  };
  routes: Route[];
  enums: Record<string, string[]>;
  breakingChanges: BreakingChange[];
}

export interface TsProperty {
  name: string;
  type: string;
  isRequired: boolean;
  summary?: string;
  nestedProperties?: TsProperty[];
}

export interface FetchParam {
  name: string;
  type: string;
  source: 'route' | 'body' | 'query';
}

export interface RouteType {
  functionName: string;
  method: string;
  path: string;
  summary?: string;
  requestTypeName?: string;
  responseTypeName?: string;
  routeParams: { name: string; type: string; isRequired: boolean }[];
  queryParams: { name: string; type: string; isRequired: boolean }[];
  fetchParams: FetchParam[];
  hasBody: boolean;
  hasQueryParams: boolean;
}

export interface TypeScriptJsonResult {
  projectName: string;
  analyzedAt: string;
  enums: Record<string, string[]>;
  interfaces: Record<string, TsProperty[]>;
  routeTypes: RouteType[];
}

export interface DocumentationResult {
  format: string;
  content: string;
  generatedAt: string;
}

// Auth types
export interface User {
  id: string;
  githubId: number;
  username: string;
  avatarUrl: string;
  email?: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  user: User;
}

// Repository types
export type ScanStatus = 'Connected' | 'Scanning' | 'Ready' | 'Failed';

export interface Repository {
  id: string;
  owner: string;
  name: string;
  fullName: string;
  defaultBranch: string;
  isPrivate: boolean;
  status: ScanStatus;
  connectedAt: string;
  lastScannedAt?: string;
  lastScan?: {
    totalRoutes: number;
    totalControllers: number;
    apiType: string;
    breakingChangeCount: number;
  };
}

// Notification types
export type NotificationType = 'BreakingChange' | 'ScanComplete' | 'ScanFailed';

export interface AppNotification {
  id: string;
  type: NotificationType;
  message: string;
  isRead: boolean;
  createdAt: string;
  repositoryId?: string;
  repositoryName?: string;
}
