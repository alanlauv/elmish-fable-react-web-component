/// Example Elmish app from https://github.com/DouglasConnect/ElmishToReact/blob/master/example/Example.fs
module CounterComponent

open Fable.Core
open Elmish
open Fable.Elmish.ElmishToReactCopy

type Props =
  [<Emit("$0.count")>] abstract Count : int
  abstract setCount : int -> unit
  // { [<Emit("$0.count")>] Count : int
  //   setCount : int -> unit }

type Model =
  { InternalCount : int
    Props : Props }

type Msg =
  | IncrementInternal
  | DecrementInternal
  | IncrementExternal
  | DecrementExternal
  | UpdateProps of Props
  | Unmount

let init props =
  { InternalCount = 0
    Props = props }

let update msg model =
  match msg with
  | IncrementInternal -> { model with InternalCount = model.InternalCount + 1 }
  | DecrementInternal -> { model with InternalCount = model.InternalCount - 1 }
  | IncrementExternal ->
      model.Props.setCount (model.Props.Count + 1)
      model
  | DecrementExternal ->
      model.Props.setCount (model.Props.Count - 1)
      model
  | UpdateProps props -> { model with Props = props }
  | Unmount ->
      Browser.Dom.console.log "unmount msg received"
      model

open Fable.React
open Fable.React.Props

let view model dispatch =
  let onClick msg =
    OnClick <| fun _ -> msg |> dispatch

  div [ ClassName "elmish" ]
    [ h3 [] [ str "The elmish program" ]
      p []
        [ str "This is the internal counter. Current value is stored in the elmish model: "
          button [ onClick DecrementInternal ] [ str "-" ]
          str (sprintf " %i "  model.InternalCount)
          button [ onClick IncrementInternal ] [ str "+" ] ]
      p []
        [ str "This is the external counter. Current value is passed to elmish program via React props:"
          button [ onClick DecrementExternal ] [ str "-" ]
          str (sprintf " %i "  model.Props.Count)
          button [ onClick IncrementExternal ] [ str "+" ] ] ]


let program = Program.mkSimple init update view

let externalisedProgram =
 Externalised.externalise program
 |> Externalised.withPropsMsg UpdateProps
 |> Externalised.withUnmountMsg Unmount
 |> Externalised.withClassName "example-class"

// NOTE: name of component is used in JS import
let CounterComponent = elmishToReact externalisedProgram




// open WebComponents
// open Browser
// open Fable.Core.JsInterop

// type WebComponent() =
//   inherit HTMLElement()

//   override this.connectedCallback() =
//       let shadowRoot = createShadowRoot()

//       importSideEffects "./counterComponent.css"

//       let style = createStyleSheetElement()
//       style.rel <- "stylesheet"
//       style.``type`` <- "text/css"
//       style.href <- "main.css"
//       shadowRoot.appendChild style

//       let mountPoint = document.createElement "div"
//       shadowRoot.appendChild mountPoint

//       let counterStart = try getAttribute "count" |> int with _ -> 0

//       Program.mkSimple init update view 
//       |> initReact mountPoint
//       |> Program.runWith { Count = counterStart; setCount = fun _ -> () } // change init to receive this record

//       retargetEvents(shadowRoot)
//       ()

// CustomElements.define "counter-component"