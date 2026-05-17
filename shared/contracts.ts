// Shared contracts — mirrors Driftless.Core.Contracts
// Used by apps/dashboard to type SignalR events.

export interface SchemaProperty {
  name: string;
  type: string;
  isRequired: boolean;
}

export interface SchemaInfo {
  typeName: string;
  properties: SchemaProperty[];
}

export interface EndpointInfo {
  method: string;
  route: string;
  controller: string;
  action: string;
  requestSchema: SchemaInfo | null;
  responseSchema: SchemaInfo | null;
}

export type ChangeKind = 'PropertyAdded' | 'PropertyRemoved' | 'TypeChanged';

export interface BreakingChange {
  endpointRoute: string;
  propertyName: string;
  kind: ChangeKind;
  oldType: string | null;
  newType: string | null;
}

// SignalR hub event payloads
export interface EndpointAddedEvent  { endpoint: EndpointInfo }
export interface EndpointChangedEvent { before: EndpointInfo; after: EndpointInfo }
export interface SchemaUpdatedEvent   { route: string; schema: SchemaInfo; changes: BreakingChange[] }
