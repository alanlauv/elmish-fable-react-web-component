/// Boilerplate code from: http://hardt.software/building-a-webcomponent-with-fable-elmish-react/
/// and https://gist.github.com/OnurGumus/2ee9ee40eb6cb5d5d185ec16ee0becb0
module WebComponents

open Fable.Core
open Elmish
open Elmish.React
open Fable.React

module CustomElements =
    [<Emit("var res = customElements.define($0,WebComponent);console.log(res)")>]
    let define (elementName:string) = jsNative

[<Global; AbstractClass>]
type HTMLElement() =
  abstract connectedCallback: unit -> unit
  default _.connectedCallback () = ()

[<Global>]
type HTMLLinkElement() =   
  member val rel : string = "" with get,set
  member val ``type`` : string = "" with get,set
  member val href : string = "" with get,set

[<Global>]
type ShadowRoot() = 
    [<Emit("$0.appendChild($1);")>]
    member this.appendChild (el:Browser.Types.HTMLElement) = jsNative
    member this.appendChild (el:HTMLLinkElement) = jsNative

[<Import("default","react-shadow-dom-retarget-events")>]
let retargetEvents shadowDom = jsNative
    
[<Emit("this.getAttribute($0);")>]
let getAttribute (name:string) : string = jsNative

[<Emit("this.attachShadow({ mode: 'open' });")>]
let createShadowRoot() : ShadowRoot = jsNative

[<Emit("document.createElement('link');")>]
let createStyleSheetElement():HTMLLinkElement = jsNative

let initReact mountPoint (program:Elmish.Program<_,_,_,_>) =
    let setState model dispatch =
        ReactDom.render(
                lazyView2With (fun x y -> obj.ReferenceEquals(x,y)) (Program.view program) model dispatch,
                mountPoint
            )
    program
    |> Program.withSetState setState