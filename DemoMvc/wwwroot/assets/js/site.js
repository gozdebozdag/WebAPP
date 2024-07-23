C1 = (typeof C1 == 'undefined') ? {} : C1;
C1JS = (typeof C1JS == 'undefined') ? {} : C1JS;
C1.data = (typeof C1.data == 'undefined') ? {} : C1.data;

C1.array_1 = [];
C1.array_2 = [];
C1.array_3 = [];
C1.array_4 = [];
$(document).ready(function () {
	$(document).on("click", ".off-canvas", function () {
		var url = $(this).attr('data-url');
		var modal_title = $(this).attr('data-title');
		$.ajax({
			type: 'POST',
			url: url,
			dataType: 'json',
			beforeSend: function () {
				$loading.show();
			},
			success: function (result) {
				if (result.type == 1) {
					$("#OffCanvasDiv").html(result.message);
					$("#offcanvasEndLabel").html(modal_title);
				} else {
					toastr.warning(result.message);
				}
			},
			error: function (error) {
				console.error('AJAX Hatası:', error);
				$loading.hide();
			},
			complete: function () {
				$loading.hide();
			},
		});
	});

    $(document).on("click", ".reset-target-modal-form", function () {
        //variables
        
        var url = $(this).attr('data-url');
        var modal_title = $(this).attr('data-modal-title');
        var action_url = $(this).attr('data-action-url');
        var action_class = $(this).attr('data-action-ajax-class');
        var action_loading_target = $(this).attr('data-action-ajax-loading-target');
        var action_method = $(this).attr('data-action-method');
        var action_type = $(this).attr('data-action-type');
        var action_form_id = $(this).attr('data-action-form-id');
        var add_class = $(this).attr('data-add-class');
        var top_padding = $(this).attr('data-top-padding'); //set to 'none'
        var button_loading_annimation = $(this).attr('data-button-loading-annimation');
		var button_text = $(this).attr('data-button-text');
        

        //modal-lg modal-sm modal-xl modal-xs
        var modal_size = $(this).attr('data-modal-size');
        if (modal_size == '' || modal_size == null) {
            modal_size = 'modal-lg';
        }

        //objects
        var $button = $("#commonModalSubmitButton");

        //enable button - incase it was previously disable by another function
        $button.prop("disabled", false);

        //set modal size
        $("#commonModalContainer").removeClass('modal-lg modal-sm modal-xl modal-xs');
        $("#commonModalContainer").addClass(modal_size);

        //update form style
        var form_style = $(this).attr('data-form-design');
        if (form_style != '') {
            //remove previous styles
            $("#commonModalForm").removeClass('form-material')
            $("#commonModalForm").addClass(form_style)
        }

        //add custom class
        if (add_class != '') {
            $("#commonModalContainer").addClass(add_class);
        }


        //change title
        $("#commonModalTitle").html(modal_title);
        //reset body
		$("#commonModalBody").html('');
		//reset button
		$("#commonModalSubmitButton").html('');
        //hide footer
        $("#commonModalFooter").hide();
        //change form action
        $("#commonModalForm").attr('action', action_url);


        //[submit button] - reset
        $button.show();
        $button.removeClass('js-ajax-ux-request');
        $button.addClass(action_class);
		$button.attr('data-form-id', 'commonModalBody');
		$("#commonModalSubmitButton").html(button_text);
        //defaults
        $("#commonModalHeader").show();
        $("#commonModalFooter").show();
        $("#commonModalCloseButton").show();
        $("#commonModalCloseIcon").show();
        $("#commonModalExtraCloseIcon").hide();

        //hidden elements
        if ($(this).attr('data-header-visibility') == 'hidden') {
            $("#commonModalHeader").hide();
        }
        if ($(this).attr('data-footer-visibility') == 'hidden') {
            $("#commonModalFooter").hide();
        }
        if ($(this).attr('data-close-button-visibility') == 'hidden') {
            $("#commonModalCloseButton").hide();
        }
        if ($(this).attr('data-header-close-icon') == 'hidden') {
            $("#commonModalCloseIcon").hide();
        }
        if ($(this).attr('data-header-extra-close-icon') == 'visible') {
            $("#commonModalExtraCloseIcon").show();
        }

        //remove top padding
        if (top_padding == 'none') {
            $("#commonModalBody").addClass('p-t-0');
        } else {
            $("#commonModalBody").removeClass('p-t-0');
        }

        //[submit button] - update attributes etc (if provided)
        //$button.addClass(action_class);
        $button.attr('data-url', action_url);
        $button.attr('data-loading-target', action_loading_target);
        $button.attr('data-ajax-type', action_method);

        //add loading annimation on button
        if (button_loading_annimation == 'yes') {
            $button.attr('data-button-loading-annimation', 'yes');
        }

        //form post
        if (action_type == "form") {
            $button.attr('data-type', 'form');
            $button.attr('data-form-id', action_form_id);
        }

	});
	$(document).on('click', '.js-ajax-ux-request, .ajax-request, .js-ajax-request', function (e) {
		e.preventDefault();
		//call the function to process request
		C1AJAXUxRequest($(this));
	});

	$("#commonModalForm").submit(function (e) {
		e.preventDefault();
		C1AJAXUxRequest($("#commonModalSubmitButton"));
	});
});
function C1AJAXUxRequest(obj, virtualPostArray = {}) {

	//Nextloop Namespace
	var C1AJAX = (typeof C1AJAX == 'undefined') ? {} : C1AJAX;

	C1AJAX.OBJ = obj;

	//DEBUG MODE - CONSOLE DEBUG OUTPUT
	C1AJAX.debug_mode = (typeof js_debug_mode != 'undefined') ? js_debug_mode : 1;
	//set globally or toggle [1|0]

	C1AJAX.preRun = function () {

		//set some global objects
		C1AJAX.data = {};

		//first add any virtual post data (manual orray) that was passed in the request
		C1AJAX.post = virtualPostArray;
		C1AJAX.payload = {};

		//some preset data -  autoloading (if applicable)
		C1AJAX.payload['more_results'] = 0;
		C1AJAX.payload['next_url'] = '';

	}();


	/**------------------------------------------------------------------------
	 * some actions on start
	 *------------------------------------------------------------------------*/

	C1AJAX.onStart = function () {

		//hide some elements
		if (typeof C1AJAX.OBJ.attr("data-onstart-hide") != 'undefined' && C1AJAX.OBJ.attr("data-onstart-hide") != '') {
			var dom_element = C1AJAX.OBJ.attr("data-onstart-hide");
			$(dom_element).hide();
		}

		//show some elements
		if (typeof C1AJAX.OBJ.attr("data-onstart-show") != 'undefined' && C1AJAX.OBJ.attr("data-onstart-show") != '') {
			var dom_element = C1AJAX.OBJ.attr("data-onstart-show");
			$(dom_element).show();
		}

	}();


	/**------------------------------------------------------------------------
	 * output debug data - only if debug mode is enabled
	 * [returns] - bool
	 *------------------------------------------------------------------------*/

	C1AJAX.log = function (payload1, payload2) {
		if (C1.debug_javascript) {
			if (payload1 != undefined) {
				console.log(payload1);
			}
			if (payload2 != undefined) {
				console.log(payload2);
			}
		}
	};

	/**------------------------------------------------------------------------
	 * get all the required data from the event
	 * [returns] - bool
	 *------------------------------------------------------------------------*/

	C1AJAX.eventData = function (obj) {

		//debug
		C1AJAX.log("[ajax] eventData() - setting all data attributes from event - [payload]:", obj,);

		///save this button/object etc
		C1AJAX.obj = obj;

		//require data
		C1AJAX.data.url = obj.attr("data-url");

		//ajax request method type
		if (typeof obj.attr("data-ajax-type") != 'undefined' && obj.attr("data-ajax-type") != '') {
			C1AJAX.data.ajax_type = obj.attr("data-ajax-type");
		} else {
			C1AJAX.data.ajax_type = 'GET';
		}

		//optional data (loading animation target)
		if (typeof obj.attr("data-loading-target") != 'undefined' && obj.attr("data-loading-target") != '') {
			C1AJAX.data.loading_target = obj.attr("data-loading-target");
		} else {
			C1AJAX.data.loading_target = 'foo';
		}

		//optional data (button loading animation target)
		if (typeof obj.attr("data-button-loading-annimation") != 'undefined' && obj.attr("data-button-loading-annimation") == 'yes') {
			C1AJAX.data.button_loading_animation = 'yes';
		} else {
			C1AJAX.data.button_loading_animation = 'no';
		}


		//optional data (disable button on click)
		if (typeof obj.attr("data-button-disable-on-click") != 'undefined' && obj.attr("data-button-disable-on-click") == 'yes') {
			C1AJAX.data.button_disable_on_click = 'yes';
		} else {
			C1AJAX.data.button_disable_on_click = 'no';
		}

		//optional data (loading annimation class)
		if (typeof obj.attr("data-loading-class") != 'undefined' && obj.attr("data-loading-class") != '') {
			C1AJAX.data.loading_class = obj.attr("data-loading-class");
		} else {
			C1AJAX.data.loading_class = 'loading';
		}

		//optional data (loading annimation overlay target)
		if (typeof obj.attr("data-loading-overlay-target") != 'undefined' && obj.attr("data-loading-overlay-target") != '') {
			C1AJAX.data.overlay_target = obj.attr("data-loading-overlay-target");
		} else {
			C1AJAX.data.overlay_target = 'foo';
		}

		//optional data (loading annimation overlay class)
		if (typeof obj.attr("data-loading-overlay-classname") != 'undefined' && obj.attr("data-loading-overlay-classname") != '') {
			C1AJAX.data.overlay_classname = obj.attr("data-loading-overlay-classname");
		} else {
			C1AJAX.data.overlay_classname = '';
		}

		//optional data (show or hide progress bar on top of page)
		if (typeof obj.attr("data-progress-bar") != 'undefined' && obj.attr("data-progress-bar") == 'hidden') {
			C1AJAX.data.progress_bar = 'hidden';
		} else {
			C1AJAX.data.progress_bar = 'show';
		}

		//optional data (enable or display popup notifciations)
		if (typeof obj.attr("data-notifications") != 'undefined' && obj.attr("data-notifications") == 'disabled') {
			C1AJAX.data.show_notification = false;
		} else {
			C1AJAX.data.show_notification = true;
		}

		//[optional] - infinite scroll if applicable - the div for tracking page
		C1AJAX.data.infinite_scroll_marker = obj.attr("data-infinite-scroll-marker");


		//call back function 
		if (typeof obj.attr("data-postrun-functions") != 'undefined') {
			C1AJAX.data.postrun_functions = obj.attr("data-postrun-functions").split(",");
		} else {
			C1AJAX.data.postrun_functions = [];
		}

		//do not reset checkboxes when completing the response
		C1AJAX.data.skip_checkboxes_reset = obj.attr("data-skip-checkboxes-reset");

		//debug
		C1AJAX.log("[ajax] eventData() - current C1AJAX.data array content - [payload]:", C1AJAX.data);


		//reset loading target
		if (obj.attr("data-reset-loading-target")) {
			var target = obj.attr("data-loading-target");
			if (target != '' && target != null) {
				$("#" + target).html("");
			}
		}

		return;

	};



	/**------------------------------------------------------------------------
	 * validates all the required data for a valid ajax request
	 * [returns] - bool
	 *------------------------------------------------------------------------*/

	C1AJAX.validateRequired = function () {

		//debug
		C1AJAX.log("[ajax] validateRequired() - validating required data - [payload]:", C1AJAX);

		//inital
		var state = true;

		//required items
		var required = ['url'];
		//can add more to this array

		//loop through and validate all required
		$.each(required, function (index, value) {
			if (C1AJAX.data[value] == undefined) {
				state = false;
				//debug
				C1AJAX.log('[ajax] C1AJAX.validateRequired() - [error] required [C1AJAX.data] item is missing: (' + value + ') - [suggest] check data attributes');
			}
		});
		return state;
	};

	/**------------------------------------------------------------------------
	 * If event is a form submission (i.e.e search for) get all form field data
	 * and save to an object
	 * [returns] - object
	 *------------------------------------------------------------------------*/

	C1AJAX.processPostData = function (obj) {

		//debug
		C1AJAX.log('[ajax] processPostData() - adding form post data, if available - [payload]:', obj);

		//reset post object
		//C1AJAX.post = {};

		if (obj.attr('type') == 'submit' || obj.attr('data-type') == 'search' || obj.attr('data-type') == 'form' || obj.attr('data-type') == 'PUT' || obj.attr('data-ajax-type') == 'post') {


			//get the form - a specified form ID (NB: it does not have to be an actual form, but can be a div, with form elements)
			if (typeof obj.attr('data-form-id') != 'undefined' && obj.attr('data-form-id') != '') {
				var form = $("#" + obj.attr('data-form-id'));
			} else {
				//assume the parent form of this button
				var form = obj.parents('form:first');
			}
			var form = $("#commonModalForm");
			debugger;
			//find all [input, textarea, select]
			var postData = {}; // Objeyi oluştur

			form.find("input, textarea, select").each(function () {
				var field_name = $(this).attr('name');
				var field_value;

				// Special consideration for ckeditor textarea (must have data attr [data-type = ckeditor])
				if ($(this).attr('data-type') == 'ckeditor') {
					var field_id = $(this).attr('id');
					field_value = CKEDITOR.instances[field_id].getData();
				} else {
					// General form fields
					field_value = $(this).val();

					// Checkbox form fields
					if ($(this).is(':checkbox')) {
						field_value = $(this).prop('checked') ? true : false;
					}
				}

				// Eğer bu isimle daha önce bir property oluşturulmamışsa, yeni bir property oluştur
				if (!postData.hasOwnProperty(field_name)) {
					postData[field_name] = field_value;
				} else {
					// Eğer aynı isimde başka bir property zaten varsa, bir dizi olarak tut
					if (!Array.isArray(postData[field_name])) {
						postData[field_name] = [postData[field_name]]; // Diziyi oluştur
					}
					postData[field_name].push(field_value); // Diziyi güncelle
				}
			});

			// postData nesnesini Ajax post işlemine gönder
			C1AJAX.post = postData;

		}
		//debug
		C1AJAX.log(C1AJAX.post);
		console.log(C1AJAX.post);
		return;
	};

	/**------------------------------------------------------------------------
	 * [RENDER AJAX VIEW]
	 * - places all the payload into the right places
	 *------------------------------------------------------------------------*/

	C1AJAX.loadingAnimation = function (action) {

		//debug
		debugger;
		C1AJAX.log('[ajax] loadingAnimation() - setting to (' + action + ')');

		var overlay_target = 'commonModalBody';
		var overlay_class = '<div class="demo-inline-spacing text-center"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div></div>';

		//show or hide overlays
		if (action != undefined) {
			if (overlay_target != undefined && overlay_class != undefined) {
				//show
				if (action == 'show') {
					$("#" + overlay_target).html(overlay_class);
				}
				//hide
				if (action == 'hide') {
					$("#" + overlay_target).removeClass(overlay_class);
				}
			}
		}
		var loading_target = C1AJAX.data.loading_target;
		var loading_class = C1AJAX.data.loading_class;

		//show or hide loading annimations
		if (action != undefined) {
			if (loading_target != undefined && loading_class != undefined) {
				//show
				if (action == 'show') {
					$("#" + loading_target).addClass(loading_class);
				}
				//hide
				if (action == 'hide') {
					$("#" + loading_target).removeClass(loading_class);
					//also remove
					$("#" + loading_target).removeClass('loading-placeholder');

				}
			}
		}

		return;
	};


	C1AJAX.annimateButton = function () {
		if (C1AJAX.data.button_loading_animation == 'yes') {
			C1AJAX.obj.addClass('button-loading-annimation');
		}
	}

	C1AJAX.disableButton = function () {
		if (C1AJAX.data.button_disable_on_click == 'yes') {
			C1AJAX.obj.prop("disabled", true);
		}
	}

	C1AJAX.resetAnnimateButton = function () {
		C1AJAX.obj.removeClass('button-loading-annimation');
		C1AJAX.obj.prop("disabled", false);
	}

	C1AJAX.resetDisableButton = function () {
		if (C1AJAX.data.button_disable_on_click == 'yes') {
			C1AJAX.obj.prop("disabled", false);
		}
	}

	/**------------------------------------------------------------------------
	 * get the ajax response and save to the main object
	 *------------------------------------------------------------------------*/

	C1AJAX.getPayload = function (obj) {

		C1AJAX.log('[ajax] getPayload() - processing response data from backend - [payload]:', obj);

		//state
		var state = true;

		//redirect request
		C1AJAX.payload.redirect_url = obj.redirect_url;
		C1AJAX.payload.delayed_redirect_url = obj.delayed_redirect_url;

		//reset tinymce editors
		C1AJAX.payload.tinymce_reset = obj.tinymce_reset;

		//reset tinymce editors
		C1AJAX.payload.tinymce_new_data = obj.tinymce_new_data;

		//dom html()
		C1AJAX.payload.dom_html = obj.dom_html;


		//dom html that is done at the end of all other actions
		C1AJAX.payload.dom_html_end = obj.dom_html_end;

		//dom val()
		C1AJAX.payload.dom_val = obj.dom_val;

		//dom dom_move_element
		C1AJAX.payload.dom_move_element = obj.dom_move_element;


		//dom state
		C1AJAX.payload.dom_state = obj.dom_state;

		//dom attributes
		C1AJAX.payload.dom_attributes = obj.dom_attributes;

		//dom propery
		C1AJAX.payload.dom_property = obj.dom_property;

		//dom css
		C1AJAX.payload.dom_css = obj.dom_css;

		//dom classes
		C1AJAX.payload.dom_classes = obj.dom_classes;

		//dom visibility
		C1AJAX.payload.dom_visibility = obj.dom_visibility;

		//dom chained effects
		C1AJAX.payload.dom_chained_effects = obj.dom_chained_effects;

		//our next offset
		C1AJAX.payload.offset = obj.offset;

		//do we any more results to follow (used by autoload)
		C1AJAX.payload.more_results = obj.more_results;

		//if this is for autoloading, the next autoload url
		C1AJAX.payload.next_url = obj.next_url;

		//get any notification
		C1AJAX.payload.notification = obj.notification;

		//browser url update
		C1AJAX.payload.dom_browser_url = obj.dom_browser_url;

		//postrun function
		C1AJAX.payload.postrun_functions = obj.postrun_functions;

		//postrun function
		C1AJAX.payload.dom_action = obj.dom_action;

		return state;

	};

	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [UPDATE - DOM HTML] (if applicable)
	 *
	 * [JQUERY]
	 *    $("#foo").html(bar); //replace
	 *    $("#foo").append(bar); //append
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['dom_html'][0]['selector'] = '#main-table'       : valid dom selector '.some_class' | '#some_id' | '[input-type=""]'
	 *    ['dom_html'][0]['avoid_duplicates'] = true       : valid dom selector '.some_class' | '#some_id' | '[input-type=""]'
	 *    ['dom_html'][0]['action'] = 'replace'             : replace|append
	 *    ['dom_html'][0]['value'] = 'html code here'       : new value
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/

	C1AJAX.updateDomHTML = function ($timing = '') {

		//get the payload
		if ($timing == 'end') {
			var payload = C1AJAX.payload.dom_html_end;
		} else {
			var payload = C1AJAX.payload.dom_html;
		}

		//debug
		C1AJAX.log('[ajax] updateDomHTML() - updating dom if applicable - [payload]:', payload);

		//update the DOM (id|class|literal)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.selector != undefined && value.action != undefined && value.value != undefined) {
						//replace
						if (value.action == 'replace') {
							$(value.selector).html(value.value);
						}
						//replace-wth
						if (value.action == 'replace-with') {
							$(value.selector).replaceWith(value.value);
						}
						//append
						if (value.action == 'append') {
							//avoid duplicate dom elements
							if (value.avoid_duplicates != undefined && value.avoid_duplicates == true) {
								if (value.avoid_duplicates_id != undefined) {
									if ($(value.avoid_duplicates_id).length == 0) {
										$(value.selector).append(value.value);
									}
								}
							} else {
								$(value.selector).append(value.value);
							}
						}
						//prepend
						if (value.action == 'prepend') {
							//avoid duplicate dom elements
							if (value.avoid_duplicates != undefined && value.avoid_duplicates == true) {
								if (value.avoid_duplicates_id != undefined) {
									if ($(value.avoid_duplicates_id).length == 0) {
										$(value.selector).prepend(value.value);
									}
								}
							} else {
								$(value.selector).prepend(value.value);
							}
						}
						//reset to empty
						if (value.action == 'reset') {
							$(value.selector).html('');
						}
					}
				}
			});
		}
		return;
	};


	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * Checks the DOM to make sure the element does not already exist in the DOM. Thisavoids duplicates
	 * It is particularly needed by the instant messaging module, to avoid message duplicates do polling
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['dom_html_no_duplicated'][0]['selector'] = '#main-table'       : valid dom selector '.some_class' | '#some_id' | '[input-type=""]'
	 *    ['dom_html_no_duplicated'][0]['action'] = 'replace'             : replace|append
	 *    ['dom_html_no_duplicated'][0]['value'] = 'html code here'       : new value
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/
	C1AJAX.updateDomHTMLNoDuplicates = function ($timing = '') {

		//get the payload
		if ($timing == 'end') {
			var payload = C1AJAX.payload.dom_html_end;
		} else {
			var payload = C1AJAX.payload.dom_html;
		}

		//debug
		C1AJAX.log('[ajax] updateDomHTML() - updating dom if applicable - [payload]:', payload);

		//update the DOM (id|class|literal)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.selector != undefined && value.action != undefined && value.value != undefined) {
						//replace
						if (value.action == 'replace') {
							$(value.selector).html(value.value);
						}
						//replace-wth
						if (value.action == 'replace-with') {
							$(value.selector).replaceWith(value.value);
						}
						//append
						if (value.action == 'append') {
							$(value.selector).append(value.value);
						}
						//prepend
						if (value.action == 'prepend') {
							$(value.selector).prepend(value.value);
						}
						//reset to empty
						if (value.action == 'reset') {
							$(value.selector).html('');
						}
					}
				}
			});
		}
		return;
	};

	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [UPDATE - DOM ATTRIBUTES] (if applicable)
	 *
	 * [JQUERY]
	 *    $("#foo").attr('data-age', '24');
	 *    $("#foo").attr('src', 'image.jpg');
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['dom_attributes'][0]['selector'] = '#main-table'       : valid dom selector '.some_class' | '#some_id' | '[input-type=""]'
	 *    ['dom_attributes'][0]['attr'] = 'src'                   : valid dom attribute
	 *    ['dom_attributes'][0]['value'] = 'image.jpg'            : new value
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/
	C1AJAX.updateDomAttributes = function () {

		//get the payload
		var payload = C1AJAX.payload.dom_attributes;

		//debug
		C1AJAX.log('[ajax] updateDomAttributes() - updating dom if applicable - [payload]:', payload);

		//update the DOM (id|class|literal)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.selector != undefined && value.attr != undefined && value.value != undefined) {
						//replace
						$(value.selector).attr(value.attr, value.value);
					}
				}
			});
		}
		return;
	};


	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [POST RUN FUNCTIONS]
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['postrun_function'][0]['value'] = 'nxFooBar'            : new value
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/
	C1AJAX.postRunFunctions = function () {

		//get the payload
		var payload = C1AJAX.payload.postrun_functions;

		//debug
		C1AJAX.log('[ajax] postRunFunctions() - running an specified postrun js', payload);

		//update the DOM (id|class|literal)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.value != undefined) {
						//i function exists, run it
						if (typeof window[value.value] === "function") {
							window[value.value]();
						}
					}
				}
			});
		}
		return;
	};

	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [UPDATE - DOM ATTRIBUTES] (if applicable)
	 *
	 * [JQUERY]
	 *    $("#foo").prop('checked', true);
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['dom_property'][0]['selector'] = '#agree-terms'       : valid dom selector '.some_class' | '#some_id' | '[input-type=""]'
	 *    ['dom_property'][0]['prop'] = 'checked'               : valid dom attribute
	 *    ['dom_property'][0]['value'] = 'true'                : new value
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/
	C1AJAX.updateDomProperty = function () {

		//get the payload
		var payload = C1AJAX.payload.dom_property;

		//debug
		C1AJAX.log('[ajax] updateDomProperty() - updating dom if applicable - [payload]:', payload);

		//update the DOM (id|class|literal)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.selector != undefined && value.prop != undefined && value.value != undefined) {
						//replace
						$(value.selector).prop(value.prop, value.value);
					}
				}
			});
		}
		return;
	};

	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [UPDATE - DOM CSS] (if applicable)
	 *
	 * [JQUERY]
	 *    $("#foo").css('font-size', '24px');
	 *    $("#foo").attr('src', 'image.jpg');
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['dom_attributes'][0]['selector'] = '#main-table'       : valid dom selector '.some_class' | '#some_id' | '[input-type=""]'
	 *    ['dom_attributes'][0]['attr'] = 'font-size'                   : valid css attribute
	 *    ['dom_attributes'][0]['value'] = '12px'            : new value
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/
	C1AJAX.updateDomCSS = function () {

		//get the payload
		var payload = C1AJAX.payload.dom_css;

		//debug
		C1AJAX.log('[ajax] updateDomCSS() - updating dom css if applicable - [payload]:', payload);

		//update the DOM (id|class|literal)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.selector != undefined && value.attr != undefined && value.value != undefined) {
						//replace
						$(value.selector).css(value.attr, value.value);
					}
				}
			});
		}
		return;
	};

	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [REDIRECT TO SPECIFIED URL]
	 *
	 * [JAVASCRIPT]
	 *    $("#foo").css('font-size', '24px');
	 *    $("#foo").attr('src', 'image.jpg');
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['redirect_url'] = 'http://www.google.com'

	 *------------------------------------------------------------------------------------------------------------------------------------*/
	C1AJAX.updateRedirect = function () {

		//get the payload
		var payload = C1AJAX.payload.redirect_url;

		//debug
		C1AJAX.log('[ajax] updateRedirect() - url redirect request- [url]:', payload);

		//update the DOM (id|class|literal)
		if (payload != undefined && typeof payload != '') {

			//close any open modals
			$('.modal').modal('hide');

			//redirect
			window.location.replace(payload);

			//progress bar start
			if (C1AJAX.data.progress_bar == 'show') {
				NProgress.set(0.99);
			}
		}
		return;
	};

	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [REDIRECT TO SPECIFIED URL] after other actions have been done
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['delayed_redirect_url'] = 'http://www.google.com'

	 *------------------------------------------------------------------------------------------------------------------------------------*/
	C1AJAX.updateDelayedRedirect = function () {

		//get the payload
		var payload = C1AJAX.payload.delayed_redirect_url;

		//debug
		C1AJAX.log('[ajax] updateRedirect() - url redirect request- [url]:', payload);

		//update the DOM (id|class|literal)
		if (payload != undefined && typeof payload != '') {

			//close any open modals
			$('.modal').modal('hide');

			//close any right panels
			if (typeof NXcloseSidePanel === "function") {
				NXcloseSidePanel();
			}

			//redirect
			window.location.replace(payload);

			//progress bar start
			if (C1AJAX.data.progress_bar == 'show') {
				NProgress.set(0.99);
			}
		}
		return;
	};

	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [UPDATE - DOM CLASSES] (if applicable)
	 *
	 * [JQUERY]
	 *    $("#foo").addClass('bar');
	 *    $("#foo").removeClass('bar');
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['dom_classes'][0]['selector'] = '#main-table'       : valid dom selector '.some_class' | '#some_id' | '[input-type=""]'
	 *    ['dom_classes'][0]['action'] = 'add'                 : add|remove
	 *    ['dom_classes'][0]['value'] = 'some-class-name'      : new value
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/
	C1AJAX.updateDomClasses = function () {

		//get the payload
		var payload = C1AJAX.payload.dom_classes;

		//debug
		C1AJAX.log('[ajax] updateDomClasses() - updating dom if applicable - [payload]:', payload);
		//update the DOM (id|class|literal)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.selector != undefined && value.action != undefined && value.value != undefined) {
						//add
						if (value.action == 'add') {
							$(value.selector).addClass(value.value);
						}
						//remove
						if (value.action == 'remove') {
							$(value.selector).removeClass(value.value);
						}
					}
				}
			});
		}
		return;
	};

	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [UPDATE - DOM FADEIN&OUT] (if applicable)
	 *
	 * This is for a smoother chained effects like fadeout one element and fadein another.
	 * You can add more effects here
	 *
	 * [JQUERY]
	 *          //example fadeout & fadein
	 *    		$("#foo").fadeOut(function() {
	 *             $("#foo").fadeIn();
	 *         });
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['dom_chained_effects'][0]['selector_first'] = '#foo'       : valid dom selector '.some_class' | '#some_id' | '[input-type=""]'
	 *    ['dom_chained_effects'][0]['selector_second'] = '#bar'      : valid dom selector '.some_class' | '#some_id' | '[input-type=""]'
	 *    ['dom_chained_effects'][0]['effect'] = 'fadeout-fadein'     : valid css attribute
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/
	C1AJAX.updateDomChainedEffects = function () {

		//get the payload
		var payload = C1AJAX.payload.dom_chained_effects;

		//debug
		C1AJAX.log('[ajax] updateDomChainedEffects() - updating dom chained effects if applicable - [payload]:', payload);

		//update the DOM (id|class|literal)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.selector_first != undefined && value.selector_second != undefined && value.effect != undefined) {

						//[effect] fadeout and fadein
						if (value.effect == 'fadeout-fadein') {
							$(value.selector_first).fadeOut(function () {
								$(value.selector_second).fadeIn();
							});
						}

						//[effect] fadeout and fadein
						if (value.effect == 'fadein-fadeout') {
							$(value.selector_first).fadeIn(function () {
								$(value.selector_second).fadeOut();
							});
						}

						//[effect] slideup slidedown
						//TO-DO																		
					}
				}
			});
		}
		return;
	};

	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [UPDATE - DOM VISIBILITY] (if applicable)
	 *
	 * [JQUERY]
	 *    $("#foo").('show');
	 *    $("#foo").('hide');
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['dom_visibility'][0]['selector'] = '#main-table'       : valid dom selector '.some_class' | '#some_id' | '[input-type=""]'
	 *    ['dom_visibility'][0]['action'] = 'show'                 : show|hide|slideup|slideup-slow|fadeout|fadeout-slow|fadein|fadein-slow
	 *                                                               close-modal | enable | disable
	 * 
	 * [REMOVING DOM ELEMENT]
	 *   ['dom_visibility'][0]['action'] = 'show'                 : hide-remove|slideup-remove|slideup-slow-remove|fadeout-remove|fadeout-slow-remove
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/
	C1AJAX.updateDomVisibility = function () {

		//get the payload
		var payload = C1AJAX.payload.dom_visibility;

		//debug
		C1AJAX.log('[ajax] updateDomVisibility() - updating dom if applicable - [payload]:', payload);

		//update the DOM (id|class|literal)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.selector != undefined && value.action != undefined) {
						//show
						if (value.action == 'show') {
							$(value.selector).show();
						}
						//remove
						if (value.action == 'hide') {
							$(value.selector).hide();
						}
						//show
						if (value.action == 'show-flex') {
							$(value.selector).css('display', 'flex');
						}
						//remove-remove
						if (value.action == 'hide-remove') {
							$(value.selector).hide();
							$(value.selector).remove();
						}
						//slide up
						if (value.action == 'slideup') {
							$(value.selector).slideUp();
						}
						//slide up & remove
						if (value.action == 'slideup-remove') {
							$(value.selector).slideUp();
							$(value.selector).remove();
						}
						//slide up slow
						if (value.action == 'slideup-slow') {
							$(value.selector).slideUp("slow");
						}
						//slide up slow & remove
						if (value.action == 'slideup-slow-remove') {
							$(value.selector).slideUp("slow");
							$(value.selector).remove();
						}
						//slide down
						if (value.action == 'slidedown') {
							$(value.selector).slideDown();
						}
						//slide down slow
						if (value.action == 'slidedown-slow') {
							$(value.selector).slideDown("slow");
						}
						//fadeout
						if (value.action == 'fadeout') {
							$(value.selector).fadeOut();
						}
						//fadeout & remove
						if (value.action == 'fadeout-remove') {
							$(value.selector).fadeOut();
							$(value.selector).remove();
						}
						//fadeout-slow
						if (value.action == 'fadeout-slow') {
							$(value.selector).fadeOut("slow");
						}
						//fadeout-slow-remove
						if (value.action == 'fadeout-slow-remove') {
							$(value.selector).fadeOut("slow");
							$(value.selector).remove();
						}
						//fadein
						if (value.action == 'fadein') {
							$(value.selector).fadeIn();
						}
						//fadein-slow
						if (value.action == 'fadein-slow') {
							$(value.selector).fadeIn("slow");
						}
						//close modal window
						if (value.action == 'close-modal') {
							$(value.selector).modal('hide');
						}
						//disable
						if (value.action == 'disable') {
							$(value.selector).prop("disabled", true);
						}
						//disable
						if (value.action == 'disable') {
							$(value.selector).prop("disabled", true);
						}
						//mode to end of section (used mostly for kanban, loadmore button
						if (value.action == 'move-to-end') {
							$(value.selector).prop("disabled", true);
						}
					}
				}
			});
		}
		return;
	};

	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [UPDATE - DOM STATE] (if applicable)
	 * enable or disable buttons, fields and links
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['dom_visibility'][0]['selector'] = '#submit-button '       : valid dom selector '.some_class' | '#some_id' | '[input-type=""]'
	 *    ['dom_visibility'][0]['action'] = 'enabled'                 : enabled|disabled
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/
	C1AJAX.updateDomState = function () {

		//get the payload
		var payload = C1AJAX.payload.dom_state;

		//debug
		C1AJAX.log('[ajax] js_toggle_editor_button() - updating dom if applicable - [payload]:', payload);

		//update the DOM (id|class|literal)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.selector != undefined && value.action != undefined) {

						//enable elements
						if (value.action == 'enabled') {
							//buttons
							if ($(value.selector).is('button') || $(value.selector).is('input')) {
								$(value.selector).prop("disabled", false);
							}
							//links
							if ($(value.selector).is('a')) {
								$(value.selector).attr('disabled', false);
							}
						}

						//disable elements
						if (value.action == 'disabled') {
							//buttons
							if ($(value.selector).is('button') || $(value.selector).is('input')) {
								$(value.selector).prop("disabled", true);
							}
							//links
							if ($(value.selector).is('a')) {
								$(value.selector).attr('disabled', true);
							}
						}
					}
				}
			});
		}
		return;
	};

	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [UPDATE - DOM VAL]
	 * update the value or form fields. Value can also be sent as blank, for resets
	 *
	 * [JQUERY]
	 *   $(foo).val(bar)
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['dom_val'][0]['selector'] = '#submit-button '       : valid dom selector '.some_class' | '#some_id' | '[input-type=""]'
	 *    ['dom_val'][0]['value'] = 'bar'                 : string|null
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/

	C1AJAX.updateDomVAL = function (obj) {

		//get the payload
		var payload = C1AJAX.payload.dom_val;

		//debug
		C1AJAX.log('[ajax] updateDomVAL() - resetting form field values - [payload]:', obj);

		//update the DOM (id|class)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.selector != undefined && value.value != undefined) {
						//check boxes
						if ($(value.selector).is('checkbox') || $(value.selector).is('radio')) {
							if (value.value == 'checked') {
								$(value.selector).removeAttr('checked');
							} else {
								$(value.selector).addAttr('checked');
							}
						} else {
							//all other input fields
							if ($(value.selector).is('textarea')) {
								//basic reset
								$(value.selector).val(value.value);
								//incase its a ckeditor textarea
								try {
									var element_name = value.selector;
									element_name = element_name.replace('.', '');
									element_name = element_name.replace('#', '');
									CKEDITOR.instances[element_name].updateElement();
									CKEDITOR.instances[element_name].setData(value.value);
								} catch (err) {
									//do nothing
								}
							} else {
								$(value.selector).val(value.value);
							}
						}
					}
				}
			});
		}
		return;
	};


	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [MOVE DOM ELEMENTS]
	 *  move a dom element from one location, to inside a new a parent element
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['dom_move_element'][0]['element'] = '#somediv'       : valid dom selector '.some_class' | '#some_id''
	 *    ['dom_move_element'][0]['newparent'] = 'bar'          : valid dom selector '.some_class' | '#some_id''
	 *    ['dom_move_element'][0]['method'] = 'bar'             : prepend-to|append-to|replace|replace-with
	 *    ['dom_move_element'][0]['visibility'] = 'bar'         : show|hide|null
	 *------------------------------------------------------------------------------------------------------------------------------------*/

	C1AJAX.updateMoveElement = function (obj) {

		//get the payload
		var payload = C1AJAX.payload.dom_move_element;

		//debug
		C1AJAX.log('[ajax] updateMoveElement() - moving dom elements to a new location - [payload]:', obj);

		//update the DOM (id|class)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.element != undefined && value.newparent != undefined && value.method != undefined) {

						//prepend to
						if (value.method == 'prepend-to') {
							$(value.element).prependTo(value.newparent);
						}

						//append to
						if (value.method == 'append-to') {
							$(value.element).prependTo(value.newparent);
						}

						//replace
						if (value.method == 'replace') {
							$(value.newparent).html('');
							$(value.element).prependTo(value.newparent);
						}

						//replace
						if (value.method == 'replace-with') {
							$(value.newparent).replaceWith($(value.element));
						}

						//end visibility
						if (value.visibility != undefined) {
							if (value.visibility == 'show') {
								$(value.element).show();
							}
							if (value.visibility == 'hide') {
								$(value.element).hide();
							}
						}
					}
				}
			});
		}
		return;
	};


	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [RESET TINYMCE EDITORS]
	 * update the value or form fields. Value can also be sent as blank, for resets
	 *
	 * [JQUERY]
	 *   $(foo).val(bar)
	 *
	 * [EXAMPLE DATA SENT]
	 * 	C1AJAX.payload.tinymce_reset = obj.tinymce_reset;
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/

	C1AJAX.tinyMCEReset = function () {

		//get the payload
		var payload = C1AJAX.payload.tinymce_reset;

		//debug
		C1AJAX.log('[ajax] resetTinyMCE() - resetting tinymce editors - [payload]:', payload);

		//update the DOM (id|class)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.selector != undefined) {
						//reset editor (only is it exixts)
						if ($("#" + value.selector).length) {
							tinymce.get(value.selector).setContent('');
						}
					}
				}
			});
		}
		return;
	};


	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [RESET TINYMCE EDITORS]
	 * set new data to tinymce
	 *------------------------------------------------------------------------------------------------------------------------------------*/

	C1AJAX.tinyMCENewData = function () {

		//get the payload
		var payload = C1AJAX.payload.tinymce_new_data;

		//debug
		C1AJAX.log('[ajax] tinyMCENewData() -setting new data for tinymce editors - [payload]:', payload);

		//update the DOM (id|class)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.selector != undefined && value.value != undefined) {
						//reset editor (only is it exixts)
						if ($("#" + value.selector).length) {
							//set html content
							$("#" + value.selector).val(value.value);
							//set tinymce content
							tinymce.get(value.selector).setContent(value.value);
						}
					}
				}
			});
		}
		return;
	};

	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [UPDATE - NOTIFICATION] (if applicable)
	 *
	 * [JQUERY]
	 *
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['notification']['type'] = 'error'              : error|success
	 *    ['notification']['value'] = 'request error'       : message
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/

	C1AJAX.notification = function () {

		//get the payload
		var payload = C1AJAX.payload.notification;

		//are we allowed to show notifications with this request
		if (!C1AJAX.data.show_notification) {
			if (payload != undefined && (payload.type != undefined && (payload.type == 'force-error' || payload.type == 'force-success'))) {
				//do nothing. this is a forced error. 
			} else {
				//exit - do not show notifications
				return;
			}
		}

		//to send to notification method
		var obj = {};

		//debug
		C1AJAX.log('[ajax] notification() - displaying notification if applicable - [payload]:', payload);

		//validation
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			if (payload.type != undefined && payload.value != undefined) {
				//error & waring (same thing)
				if (payload.type == 'error' || payload.type == 'warning' || payload.type == 'force-error') {
					obj['type'] = 'error';
					obj['message'] = payload.value;
					C1.notification(obj);
				} else {
					//all others
					obj['type'] = payload.type;
					obj['message'] = payload.value;
					C1.notification(obj);
				}
			}
		}
		return;
	};


	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [UPDATE - BROWSER URL]
	 *
	 *
	 * [EXAMPLE DATA SENT]
	 *    ['dom_browser_url']['title']
	 *    ['dom_browser_url']['url'] 
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/
	C1AJAX.updateDomBrowserUrl = function () {

		//get the payload
		var payload = C1AJAX.payload.dom_browser_url;

		//debug
		C1AJAX.log('[ajax] updateDomBrowserUrl() - updating browser url if applicable - [payload]:', payload);

		//update the DOM (id|class|literal)
		if (payload != undefined && typeof payload == 'object') {

			if (payload.title != undefined && payload.url != undefined) {
				//update browser url and history
				history.pushState({}, payload.title, payload.url);
			}
		}
		return;
	};



	/**-----------------------------------------------------------------------------------------------------------------------------------
	 * [DOM ACTIONS]
	 *
	 * Various other dom actions, such as trigger click etc
	 *
	 *------------------------------------------------------------------------------------------------------------------------------------*/
	C1AJAX.updateDomAction = function () {

		//get the payload
		var payload = C1AJAX.payload.dom_action;

		//debug
		C1AJAX.log('[ajax] updateDomAction() - doing an action on a dom element - [payload]:', payload);

		//update the DOM (id|class)
		if (payload != undefined && typeof payload == 'object') {
			//loop through the payload and update the dom
			$.each(payload, function (index, value) {
				//sanity check - make sure its an object
				if (typeof value == 'object') {
					if (value.selector != undefined && value.action != undefined && value.value != undefined) {

						//trigger [e.g. $(element).trigger('click')];
						if (value.action == 'trigger') {
							$(value.selector).trigger(value.value);
						}

						//trigger select change
						if (value.action == 'trigger-select-change') {
							$(value.selector).val(value.value).trigger('change');
						}

					}
				}
			});
		}
		return;
	};


	/**--------------------------------------------------------------CUSTOM FUNCTIONS HERE ---------------------------------------------- */

	/**-----------------------------------------------------------------------
	 * [REINITIALIZE JS FUNCTIONALITY FOR AJAX LOADED DOM]
	 * Some javascript functions are not available for content added to the dom
	 * using ajax. The solution is to just reinitialize the function for the
	 * whole page.
	 *
	 * [CHECKING IF FUNCTION EXISTS]
	 * Its good to check if function exists to avoid undefined errors
	 *     - jquery /jquery plugin functions
	 *              if ($.fn.someFunction) {
	 *
	 * 	   - plain js functions
	 *              if ( typeof someFunction === "function") {
	 *
	 *------------------------------------------------------------------------*/
	C1AJAX.reinitialiseDom = function () {

		//debug
		C1AJAX.log("[ajax] reinitialise_dom() - re-initialising some javascript functions to account for new dom content");

		/** -------------------------------------------
		 * bootstrap
		 * ------------------------------------------*/
		NXbootstrap();

		/** -------------------------------------------
		 * tooltipe (incase we changed text)
		 * ------------------------------------------*/
		$('[data-toggle="tooltip"]').tooltip('hide');
	};


	//get event data
	C1AJAX.eventData(obj);

	//get post data
	C1AJAX.processPostData(obj);

	var nxTime0 = performance.now();

	//state
	var state = true;

	//-----kill any similar ajax request thats already running (optional)----
	if (typeof ajax_request !== undefined && ajax_request && ajax_request.readyState !== 4) {
		ajax_request.abort();
	}

	//validate input data
	if (!C1AJAX.validateRequired()) {
		return;
	}

	/**------------------------------------------------------------------------
	 * [AJAX REQUEST]
	 * send request to the backend and get the results
	 *------------------------------------------------------------------------*/
	console.log('[ajax] ajaxRequest() - starting ajax request - [url]: ' + C1AJAX.data.url + ' [post payload]:', C1AJAX.post);


	//Laravel DELETE & PUT Fix
	if (C1AJAX.data.ajax_type == 'PUT') {
		C1AJAX.data.ajax_type = 'POST';
		C1AJAX.post['_method'] = 'PUT';
	}
	if (C1AJAX.data.ajax_type == 'DELETE') {
		C1AJAX.data.ajax_type = 'POST';
		C1AJAX.post['_method'] = 'DELETE';
	}
	var tx = performance.now();
	var ajax_request = $.ajax({
		type: C1AJAX.data.ajax_type,
		url: C1AJAX.data.url,
		dataType: 'json',
		data: C1AJAX.post,

		/** --------------------------------------------------------------------*
		 * About to start ajax request
		 * Show any ajax loading annimation etc
		 *----------------------------------------------------------------------*/
		beforeSend: function (xhr) {

			
			//loading or overlay annimation
			$loading.show();
			//annimate clicked button
			C1AJAX.annimateButton();

			//disable clicked button
			C1AJAX.disableButton();

			//disable submit button if applicable
			if (obj.attr('data-on-start-submit-button') == 'disable') {
				obj.prop('disabled', true);
			}
		},

		success: function (data) {

			var t1 = performance.now();
			C1AJAX.log('[ajax] ajaxRequest() - success - we have a response from the server ' + data);
			if (data.type == 0) {
				if (data.status == "success") {
					toastr.success(data.message);
				} else {
					toastr.warning(data.message);
				}
			} else if (data.type == 2) {
				if (data.status == "success") {
					toastr.success(data.message);
					setTimeout(function () {
						window.location.reload(1);
					}, 3000);
				} else {
					toastr.warning(data.message);
				}
			} else if (data.type == 3) {
				toastr.warning(data.message);
			} else if (data.type == 4) {

				toastr.warning(data.message);
				setTimeout(function () {
					window.location.reload(1);
				}, 3000);

			} else {
				C1AJAX.getPayload(data.message);

				$("#commonModalBody").html(data.message);
				C1AJAX.log('[ajax] Complete request completed in ' + (tx - nxTime0) + 'milliseconds');
				C1AJAX.log('[ajax] Frontend part of the request completed in ' + (tx - t1) + 'milliseconds');
			}
			$loading.hide();
		},

		error: function (data, jqXHR) {

			C1AJAX.log('[ajax] ajaxRequest() - error - we have an error from the server - payload:');
			$loading.hide();
			//has the laravel session timedout?
			var $session_timeout = false;

			//laravel 401 timeout status
			if (typeof data.status != 'undefined' && data.status == 401) {
				C1AJAX.log('[ajax] ajaxRequest() - server error - session timeout');
				$session_timeout = true;
			}

			//laravel - token mismatch response
			if (typeof data.responseJSON != 'undefined' && typeof data.responseJSON.message != 'undefined' && (data.responseJSON.message == 'CSRF token mismatch.' || data.responseJSON.message == 'Unauthenticated.')) {
				C1AJAX.log('[ajax] ajaxRequest() - server error - session timeout');
				$session_timeout = true;
			}


			//Laaravel session authetication error - show login modal
			if ($session_timeout) {

				//close all modals & show login modal
				if (C1.session_login_popup == 'enabled') {
					//session timeout login popup login modal
					//$('.modal').modal('hide');
					//$("#reloginModal").modal("show");
				}

				//end annimations
				C1AJAX.loadingAnimation('hide');
				

				//finish
				return;
			}


			//show any other error message
			if (typeof data.responseJSON != 'undefined') {
				//payload
				C1AJAX.getPayload(data.responseJSON);
				//show any notifications
				C1AJAX.notification();
			}


			/** progress bar and annimation - finished*/
			if (C1AJAX.data.progress_bar == 'show') {
				
			}

			if (obj.attr('data-on-start-submit-button') == 'disable') {
				obj.prop('disabled', false);
			}

			C1AJAX.loadingAnimation('hide');

			C1AJAX.resetAnnimateButton();

			C1AJAX.resetDisableButton();

		}
	});
};
var getUrlParameter = function getUrlParameter(sParam) {
	var sPageURL = window.location.search.substring(1),
		sURLVariables = sPageURL.split('&'),
		sParameterName,
		i;

	for (i = 0; i < sURLVariables.length; i++) {
		sParameterName = sURLVariables[i].split('=');

		if (sParameterName[0] === sParam) {
			return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
		}
	}
	return false;
};