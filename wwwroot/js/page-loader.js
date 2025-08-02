// Page Loader - Handles page loading animations
class PageLoader {
    constructor() {
        this.loader = null;
        this.isEnabled = true;
        this.loaderType = 'spinner';
        this.init();
    }

    init() {
        // Create loader element
        this.createLoader();
        
        // Show loader on page load
        if (this.isEnabled) {
            this.show();
        }
        
        // Hide loader when page is fully loaded
        window.addEventListener('load', () => {
            this.hide();
        });
        
        // Handle navigation events
        this.handleNavigation();
    }

    createLoader() {
        // Remove existing loader if any
        const existingLoader = document.querySelector('.page-loader');
        if (existingLoader) {
            existingLoader.remove();
        }

        // Create new loader
        this.loader = document.createElement('div');
        this.loader.className = 'page-loader';
        
        // Create loader content based on type
        const loaderContent = this.createLoaderContent();
        this.loader.appendChild(loaderContent);
        
        // Add to body
        document.body.appendChild(this.loader);
    }

    createLoaderContent() {
        const container = document.createElement('div');
        
        switch (this.loaderType) {
            case 'spinner':
                const spinner = document.createElement('div');
                spinner.className = 'loader-spinner';
                container.appendChild(spinner);
                break;
                
            case 'dots':
                container.className = 'loader-dots';
                for (let i = 0; i < 3; i++) {
                    const dot = document.createElement('div');
                    dot.className = 'dot';
                    container.appendChild(dot);
                }
                break;
                
            case 'bars':
                container.className = 'loader-bars';
                for (let i = 0; i < 4; i++) {
                    const bar = document.createElement('div');
                    bar.className = 'bar';
                    container.appendChild(bar);
                }
                break;
                
            case 'pulse':
                const pulse = document.createElement('div');
                pulse.className = 'loader-pulse';
                container.appendChild(pulse);
                break;
                
            default:
                const defaultSpinner = document.createElement('div');
                defaultSpinner.className = 'loader-spinner';
                container.appendChild(defaultSpinner);
        }
        
        return container;
    }

    show() {
        if (this.loader && this.isEnabled) {
            this.loader.classList.remove('hidden');
        }
    }

    hide() {
        if (this.loader) {
            this.loader.classList.add('hidden');
            // Remove loader after fade out
            setTimeout(() => {
                if (this.loader && this.loader.parentNode) {
                    this.loader.remove();
                }
            }, parseFloat(getComputedStyle(document.documentElement).getPropertyValue('--loader-fade-out-duration')) * 1000);
        }
    }

    handleNavigation() {
        // Show loader on navigation
        if (this.isEnabled) {
            // Handle browser back/forward
            window.addEventListener('beforeunload', () => {
                this.show();
            });
            
            // Handle internal navigation (if using AJAX)
            document.addEventListener('click', (e) => {
                const link = e.target.closest('a');
                if (link && link.href && !link.href.startsWith('javascript:') && !link.href.startsWith('#')) {
                    this.show();
                }
            });
        }
    }

    // Update loader settings
    updateSettings(settings) {
        this.isEnabled = settings.enablePageLoader || false;
        this.loaderType = settings.loaderType || 'spinner';
        
        // Update CSS variables
        const root = document.documentElement;
        root.style.setProperty('--loader-color', settings.loaderColor || '#007bff');
        root.style.setProperty('--loader-background-color', settings.loaderBackgroundColor || '#ffffff');
        root.style.setProperty('--loader-size', settings.loaderSize || '40px');
        root.style.setProperty('--loader-animation-duration', settings.loaderAnimationDuration || '1s');
        root.style.setProperty('--loader-fade-out-duration', settings.loaderFadeOutDuration || '0.5s');
        
        // Recreate loader with new settings
        this.createLoader();
    }

    // Method to show loader manually
    showLoader() {
        if (this.isEnabled) {
            this.show();
        }
    }

    // Method to hide loader manually
    hideLoader() {
        this.hide();
    }
}

// Initialize page loader when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    window.pageLoader = new PageLoader();
    
    // Check if theme settings are available from the page
    if (window.themeSettings) {
        window.pageLoader.updateSettings({
            enablePageLoader: window.themeSettings.enablePageLoader || false,
            loaderType: window.themeSettings.loaderType || 'spinner',
            loaderColor: window.themeSettings.loaderColor || '#007bff',
            loaderBackgroundColor: window.themeSettings.loaderBackgroundColor || '#ffffff',
            loaderSize: window.themeSettings.loaderSize || '40px',
            loaderAnimationDuration: window.themeSettings.loaderAnimationDuration || '1s',
            loaderFadeOutDuration: window.themeSettings.loaderFadeOutDuration || '0.5s'
        });
    }
});

// Export for use in other scripts
if (typeof module !== 'undefined' && module.exports) {
    module.exports = PageLoader;
} 