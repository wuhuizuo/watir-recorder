#  WatirRecorder is a utility for recording Watir scripts.  
#  Watir is a web application testing tool for Ruby.
#  Home page is http://www.watir.org
#
#  Version "Revision: 0.2"
#
#  Console Version:
#  
#  Typical usage: 
#   Ruby watir_recorder.rb                -> prints the script to standard out
#   Ruby watir_recorder.rb > MyScript.rb  -> redirects output to a file

require 'win32ole'

##/////////////////////////////////////////////////////////////////////////////////////////////////////
##
## Main WatirRecorder class.  It handles the IE automation, which primarily means event handling.
##
##/////////////////////////////////////////////////////////////////////////////////////////////////////
class WatirRecorder 

   TOP_LEVEL_FRAME_NAME = ""


   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Initializer executed when "new" is called.
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def initialize
      
      # contains references to all document objects that we're listening to
      @activeDocuments = Hash.new
      
      # HACK: tells browser_BeforeNavigate2 method whether or not to insert a goto navigation statement
      # This is set false when the user clicks something, and reset to true when we get a
      # DocumentComplete event. This will be problematic if user clicks a button that does not affect
      # navigation, and then navigates manually.
      @navigateDirectly = true
      
      # stores the last frame name      
      @lastFrameName = ""
      setDebugLevel(2)
   end


   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Starts recording all interactions with IE.
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def startRecording
   
      # initialize IE
      @ie = WIN32OLE.new( 'InternetExplorer.Application' )
      @ie.visible = TRUE

      browserEvents = WIN32OLE_EVENT.new( @ie, 'DWebBrowserEvents2' )
      browserEvents.on_event { |*args| browserEventHandler( *args ) }
      
      # print script header
      puts "##//////////////////////////////////////////////////////////////////////////////////////////////////"
      puts "##"
      puts "## Watir script recorded by WatirRecorder."
      puts "##"
      puts "##//////////////////////////////////////////////////////////////////////////////////////////////////"
      puts ""
      puts "require 'watir'"
      puts "include Watir"
      puts ""
      puts "browser = IE.new"
      # puts "ie.set_fast_speed()" this doesn't seem to work so well with multiple frames
      puts ""

      # capture events
      catch( :done ) {
         loop {
            WIN32OLE_EVENT.message_loop
         }
      }
      
      # IE takes a moment to close.  
      # Making it invisible in the interim produces a slightly nicer user experience.
      @ie.visible = FALSE
   end


   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Dispatches all events from the browser to their handlers.
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def browserEventHandler( event, *args )
      case event
      when "BeforeNavigate2"
         browser_BeforeNavigate2( args )       
         
      when "NavigateComplete2"
         #printDebugComment( "EVENT: NavigateComplete2" )
         #printNavigateComment( args[1], nil )   
      when "DocumentComplete"
          browser_DocumentComplete( args )
         
      when "OnQuit"
          browser_OnQuit( args )
      end
   end


   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Handles the browser's BeforeNavigate2 event.
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def browser_BeforeNavigate2( args )
      #printDebugComment( "EVENT: BeforeNavigate2" )
      
      url       = args[1]
      frameName = args[3]
      
      if frameName == TOP_LEVEL_FRAME_NAME
         @frameNames = Array.new
         
         if @navigateDirectly == true
			if not url.match("javascript:'';")
				puts "browser.goto( '" + url + "' )"
			end
         end
      end
      
      @frameNames << frameName
      
      # printNavigateComment( url, frameName )
   end


   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Handles the browser's DocumentComplete event.
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def browser_DocumentComplete( args )
      #printDebugComment( "EVENT: DocumentComplete" )
      
      # reset the navigateDirectly flag
      
      # TODO: This does not currently handle multiple frames with the same name.  It will
      #       be unable to get the document in that case.  It currently prints a warning.
      webBrowser = args[0]
               
      begin
         # HACK: The next line is used to determine whether or not the top-level document has
         #       loaded.  One would think that you could simply check "webBrowser.document == nil",
         #       but this does not work.  Accessing webBrowser.document prior to the DocumentComplete
         #       event for the top-level document causes an "Unknown property or method `document'"
         #       exception.
         document = webBrowser.document
         
         # if frameNames is nil, we won't go very far here...
         if @frameNames == nil
            return
         end

         # iterate through the frameNames array and put each frame's document in the documents
         # hash and register all supported event handlers for that document.
         @frameNames.each { |frameName|         
            
            if frameName == TOP_LEVEL_FRAME_NAME
               document = webBrowser.document
               @navigateDirectly = true
            else
               begin
                  document = webBrowser.document.frames[frameName].document
               rescue
                  printWarningComment( "Unable to get the document for frame (" + frameName + ")." )
                  return
               end
            end

            # TODO: When we get around to handling multiple frames with the same name, we'll want
            #       the documentKey to be the concatenation of the frame name and index.
            documentKey = frameName

            if ( webBrowser.Type == "HTML Document"  && !@activeDocuments.has_key?( documentKey ) )
               
               # create a new document object in the hash
               @activeDocuments[documentKey] = WIN32OLE_EVENT.new( document, 'HTMLDocumentEvents2' )
               
               # register event handlers
               @activeDocuments[documentKey].on_event( 'onclick' ) { |*args| document_onclick( args[0] ) }
               @activeDocuments[documentKey].on_event( 'onfocusout' ) { |*args| document_onfocusout( args[0] ) }               
            end
         
         }
      rescue WIN32OLERuntimeError => e
         if e.to_s.match( "nknown property or method `document'" )
            return
         else
            puts printWarningComment("Exception occured while loading document:\n" + e.to_s)
         end
      end
   end


   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Handles the browser's OnQuit event.
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def browser_OnQuit( args )      
      throw :done
   end


   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Handles document onclick events.
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def document_onclick( eventObj )            
      # if the user clicked something and the URL chandes as a result, it's probably due to this...
      @navigateDirectly = false
      
      case eventObj.srcElement.tagName      
        when "INPUT", "A", "SPAN", "IMG", "TD"
           writeWatirStatement( eventObj, "click" )           
        else
           how, what = getHowWhat( eventObj.srcElement )
           printDebugComment( "Unsupported onclick tagname " + eventObj.srcElement.tagName + 
                              " (" + how + ", '" + what + "')" )         
     end
   end


   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Print Watir statement for onfocusout events.
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def document_onfocusout( eventObj ) 
      case eventObj.srcElement.tagName
        when "INPUT"
           case eventObj.srcElement.getAttribute( "type" )
              when "text", "password"
                 writeWatirStatement( eventObj, "set", eventObj.srcElement.value )
              when "checkbox"
                 if eventObj.srcElement.checked
                    writeWatirStatement( eventObj, "set" )
                 else
                    writeWatirStatement( eventObj, "clear" )
                 end
              else               
            printDebugComment( "Unsupported onfocusout INPUT type " + \
                                    eventObj.srcElement.getAttribute( "type" ))
           end
           
        when "SELECT"
           puts writeWatirStatement( eventObj, "select", eventObj.srcElement.value )
           
        else
           how, what = getHowWhat( eventObj.srcElement )
           printDebugComment( "Unsupported onfocusout tagname " + eventObj.srcElement.tagName + 
                              " (" + how + ", '" + what + "')" )  
      end
   end
   

   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Generates a line of WATIR script based on the HTML element and the action to take.
   ##
   ## element: The IE HTML element to perform the action on.
   ## action:  The WATIR action to perform on said element.
   ## value:   The value to assign to the element (required for 'set' and 'select' actions)
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def writeWatirStatement( eventObj, action, value = "" )
      str = ""         
      element = eventObj.srcElement   
      if element == nil
         printDebugComment "writeWatirStatement eventObj.srcElement was nil!"
         return str
      end
      
      # if we're trasitioning between frames, insert a delay statement
      if @lastFrameName != element.document.parentWindow.name
         # TODO: How do we do a Thread.Wait() in Ruby?
      end
          
      case element.tagName
      
         when "INPUT"
            case element.getAttribute( "type" )
               when "submit", "image", "button"
                  if action == "click"
                     str = genWatirAccessor( "button", element ) + action
                  end
                     
               when "text", "password"
                  if action == "set"
                     str = genWatirAccessor( "text_field", element ) + action + "( '" +  element.value + "' )"
                  end
                 
               when "checkbox"
                  if action == "set" || action == "clear"
                     str = genWatirAccessor( "checkbox", element ) + action
                  end
                  
               when "radio"
                  if action == "set" || action == "clear"
                     str = genWatirAccessor( "radio", element ) + action
                  end
                 
               else
                  how, what = getHowWhat( eventObj.srcElement )
                  printDebugComment( "Unsupported INPUT type " + element.getAttribute( "type" ) + 
                                     " (" + how + ", '" + what + "')" )
            end
            
         when "A"
            if action == "click"
               str = genWatirAccessor( "link", element ) + action
            end            
            
         when "SPAN"
            if action == "click"
          str = genWatirAccessor( "span", element ) + action
            end
            
         when "IMG"            
            if action == "click"
                str = genWatirAccessor( "image", element ) + action
            end
            
         when "TD"
            if action == "click"
               how, what = getHowWhat( element )
               str = genIePrefix( element ) + "document.all[ '" + what + "' ].click"
            end
            
         when "SELECT"
            if action == "select"
               for i in 0..element.options.length-1
                  if element.options[ %Q{#{i}} ].selected
                     str += genWatirAccessor( "select_list", element ) + action + "( '" + \
                               element.options[ %Q{#{i}} ].text + "' )\n"
                  end
               end
            end
            
         else
            how, what = getHowWhat( eventObj.srcElement )
            printDebugComment( "Unsupported onclick tagname " + eventObj.srcElement.tagName + 
                               " (" + how + ", '" + what + "')" )         
      end
      
      
      if str != ""
         @lastFrameName = element.document.parentWindow.name
      else
         printDebugComment "Unsupported action '" + action + "' for '" + element.tagName + "'."
      end
      
      puts str
   end
   
   
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Generates the WATIR code necessary for accessing a particular document element.
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def genWatirAccessor( watirType, element )
        
      iePrefix = genIePrefix( element )
      how, what = getHowWhat( element )
            
      # for some reason the index 'How' doesn't work for the index we get from our code
      if how == ":index"
         return iePrefix + "document.all[ '" + what + "' ]."
      elsif how == ":id"
      	if what.include? "_"
			what = what[what.rindex("_") + 1,what.length - what.rindex("_")]
			what = "/" + what + "$/"
		 else
			what = "'" + what + "'"
		 end
         return iePrefix + watirType + "( " + how + ", " + what + " )."
      else
         return iePrefix + watirType + "( " + how + ", '" + what + "' )."
      end
   end
   
   
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Generates the ie prefix necessary for accessing a particular document element, including frames.
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def genIePrefix( element )   
		printDebugComment("genIePrefix")
      parentWindowName = element.document.parentWindow.name
      
      if parentWindowName != TOP_LEVEL_FRAME_NAME
         return "browser.frame( :name, '" + parentWindowName + "' )."
      else
         return "browser."
      end
   end
   
   
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Returns the "how" and the "what" values for accessing an IE object with WATIR.
   ##
   ## Note: may need different getHowWhat methods for different element types because each element 
   ##       type supports different "how" values.
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def getHowWhat( element )

      if element.getAttribute( "id" ) != nil && element.getAttribute( "id" ) != ""
         return ":id", element.getAttribute( "id" )
      elsif element.getAttribute( "name" ) != nil && element.getAttribute( "name" ) != ""
         return ":name", element.getAttribute( "name" )
      else
		printDebugComment(element.tagName)      
		case element.tagName
		when "A"
			return ":text", %Q{#{element.innerText}}
		else
         #return the index of this element in the 'all' collection as a string
         index = element.sourceIndex
         if index != nil
            return ":index", %Q{#{index}}
         end
		end
      end
   end
   

   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Print warning comment.
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def printWarningComment( warning )
      if @printWarnings
		  puts ""
		  puts "# WARNING: '" + warning
		  puts ""
      end
   end


   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Print a debug message comment.
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def printDebugComment( message )
	  if @printDebugInfo
		puts "# DEBUG: " + message
	  end
   end


   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   ##
   ## Print comment showing the page navigated to.
   ##
   ##//////////////////////////////////////////////////////////////////////////////////////////////////
   def printNavigateComment( url, frameName )
      puts ""
      puts "# frame loading: '" + frameName + "'" if frameName != nil
      puts "# navigating to: '" + url + "'"
      puts ""
   end
   
   def setDebugLevel(level)
		if level >= 2
			@printDebugInfo = true
		else
			@printDebugInfo = false			
		end
		if level >= 1
			@printWarnings = true
		else
			@printWarnings = false
		end
   end
end  # end class


##//////////////////////////////////////////////////////////////////////////////////////////////////
##
## Begin script.
##
##//////////////////////////////////////////////////////////////////////////////////////////////////
wm = WatirRecorder.new
#Only show warnings
wm.setDebugLevel(1)
wm.startRecording