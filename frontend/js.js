//let modalId = $('#image-gallery');
var page = 1;
var filter = "Any";
var host = "opossum-gallery.herokuapp.com";

$(document).ready(function () {
	loadListPhoto(page,filter);
	//loadGallery(true, 'a.thumbnail');

    //This function disables buttons when needed
    function disableButtons(counter_max, counter_current) {
      $('#show-previous-image, #show-next-image')
        .show();
      if (counter_max === counter_current) {
        $('#show-next-image')
          .hide();
      } else if (counter_current === 1) {
        $('#show-previous-image')
          .hide();
      }
    }
	
	// Function for downloading photo from loremphoto
	function loadListPhoto(page,filter){
		if (filter=="Any")
		{
			var pathRequest = "https://" + host + "/api/PhotosPagination/GetAllPhotos?pageNumber=" + page + "&emotion=" + filter + "&maxRequired=50";
			$.get(pathRequest, function(data, status){
				if (status=='success') {
					if (data.length < 50) {
						$("#next-page").hide();
						$("#mistakes").html("<p>No more photos with " + filter + "</p>");
					}
					else {
						$("#next-page").show();
						$("#mistakes").html("");
					}
					var photogrid = '';
					var photo;
					 for(photo in data) {
						var path = data[photo].url;
						photogrid += '<div class="col-lg-3 col-md-4 col-xs-6 thumb filter">' +
				'<a class="thumbnail" href="#" data-image-id="" data-toggle="modal" data-title="" data-image="' + 
				path + '" data-target="#image-gallery"> <img class="img-thumbnail" src="' + path + 
				'" > </a> </div>';
					}
					if (page==1) $('#photo-grid').append(photogrid)
						else $('#photo-grid').append(photogrid);
					}
				else if (status=='nocontent'){
					$("#next-page").hide();
					$("#mistakes").html("<p>No more photos </p>");
					}
				else {
					$("#mistakes").html("<p>Something going wrong :c</p>");
				}
				loadGallery(true, 'a.thumbnail');
				});
				
		} else 
		{
			var pathRequest = "https://" + host + "/api/PhotosPagination/GetPhotosWithMostEmotions?pageNumber=" + page + "&emotion=" + filter + "&maxRequired=50";
			$.get(pathRequest, function(data, status){
				if (status=='success') {
					
					if (data.length < 50) {
						$("#next-page").hide();
						$("#mistakes").html("<p>No more photos with " + filter + "</p>");
					}
					else {
						$("#next-page").show();
						$("#mistakes").html("");
					}
					var photogrid = '';
					var photo;
					 for(photo in data) {
						var path = data[photo].url;
										
						photogrid += '<div class="col-lg-3 col-md-4 col-xs-6 thumb filter">' +
				'<a class="thumbnail" href="#" data-image-id="" data-toggle="modal" data-title="" data-image="' + 
				path + '" data-target="#image-gallery"> <img class="img-thumbnail" src="' + path + 
				'" > </a> </div>';
					}
					if (page==1) $('#photo-grid').html(photogrid);
						else $('#photo-grid').append(photogrid);
					}
					
				else if (status=='nocontent'){
					$("#next-page").hide();
					$("#mistakes").html("<p>No more photos with " + filter + "</p>");
					}
				else {
					$("#mistakes").html("<p>Something going wrong :c</p>");
				}
				loadGallery(true, 'a.thumbnail');
				});
				
		}
	}
	
    /**
     *
     * @param setIDs        Sets IDs when DOM is loaded. If using a PHP counter, set to false.
     * @param setClickAttr  Sets the attribute for the click handler.
     */
    function loadGallery(setIDs, setClickAttr) {
      let current_image,
        selector,
        counter = 0;

      $('#show-next-image, #show-previous-image')
        .click(function () {
          if ($(this)
            .attr('id') === 'show-previous-image') {
            current_image--;
          } else {
            current_image++;
          }
          selector = $('[data-image-id="' + current_image + '"]');
          updateGallery(selector);
        });

      function updateGallery(selector) {
        let $sel = selector;
        current_image = $sel.data('image-id');
        $('#image-gallery-title')
          .text($sel.data('title'));
        $('#image-gallery-image')
          .attr('src', $sel.data('image'));
        disableButtons(counter, $sel.data('image-id'));
      }

      if (setIDs == true) {
        $('[data-image-id]')
          .each(function () {
            counter++;
            $(this)
              .attr('data-image-id', counter);
          });
      }
      $(setClickAttr)
        .on('click', function () {
          updateGallery($(this));
        });
    }
	
	$("#next-page").click(function(){
		page++;
		loadListPhoto(page, filter);
    });
	
	// Filter button
	 $(".filter-button").click(function(data){
      filter = $(this).attr('data-filter');
	  $(".active:first").removeClass("active");
	  $(this).addClass("active");
	  page=1;	  
      loadListPhoto(page, filter);
		});
	
  });

// build key actions
$(document).keyup(function (e) {
    switch (e.which) {
      case 37: // left
        if (($('#image-gallery').data('bs.modal') || {})._isShown && $('#show-previous-image').is(":visible")) {
          $('#show-previous-image').click();
        }
        break;
		
      case 39: // right
        if (($('#image-gallery').data('bs.modal') || {})._isShown && $('#show-next-image').is(":visible")) {
          $('#show-next-image').click();
        }
        break;
      default:
        return; // exit this handler for other keys
    }
    e.preventDefault(); // prevent the default action (scroll / move caret)
  });
