module Messages

open System

type Token = Guid 
type Angle = float
type Length = int
type Sides = int
type Position = int * int

type TurtleCommand = 
  | Turn of Angle
  | Draw of Length
  | Move of Position
  | Polygon of Sides * Length
  | Color of byte * byte * byte

type Request = 
  | Register of string 
  | TurtleCommand of Token * TurtleCommand
  | Ping

type Response = 
  | Registered of Token
  | TurtleCommandExecuted
  | Pong
  | UnknownToken of Token
  | RegistrationFailed