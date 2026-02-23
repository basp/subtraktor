namespace Subtraktor

type IoError =
    | FileNotFound of path: string
    | AccessDenied of path: string
    | DecodeFailed of path: string * reason: string
    | EncodeFailed of path: string * reason: string
    | DeviceUnavailable of deviceId: string

type AudioError =
    | Validation of ValidationError
    | Io of IoError
    | Unexpected of message: string
