// Theme Loader - Loads and applies theme settings from database
class ThemeLoader {
    constructor() {
        this.themeSettings = null;
        this.init();
    }

    async init() {
        try {
            // Load theme settings from server
            const response = await fetch('/Admin/GetThemeSettings');
            if (response.ok) {
                this.themeSettings = await response.json();
                this.applyTheme();
            } else {
                console.warn('Could not load theme settings, using defaults');
                this.applyDefaultTheme();
            }
        } catch (error) {
            console.error('Error loading theme settings:', error);
            this.applyDefaultTheme();
        }
    }

    applyTheme() {
        if (!this.themeSettings) {
            this.applyDefaultTheme();
            return;
        }

        const root = document.documentElement;
        
        // Apply color settings
        root.style.setProperty('--primary-color', this.themeSettings.primaryColor || '#007bff');
        root.style.setProperty('--secondary-color', this.themeSettings.secondaryColor || '#6c757d');
        root.style.setProperty('--background-color', this.themeSettings.backgroundColor || '#ffffff');
        root.style.setProperty('--text-color', this.themeSettings.textColor || '#333333');
        
        // Apply button gradient settings
        root.style.setProperty('--primary-button-gradient-start', this.themeSettings.primaryButtonGradientStart || '#007bff');
        root.style.setProperty('--primary-button-gradient-end', this.themeSettings.primaryButtonGradientEnd || '#0056b3');
        root.style.setProperty('--secondary-button-gradient-start', this.themeSettings.secondaryButtonGradientStart || '#6c757d');
        root.style.setProperty('--secondary-button-gradient-end', this.themeSettings.secondaryButtonGradientEnd || '#545b62');
        root.style.setProperty('--outline-button-border-color', this.themeSettings.outlineButtonBorderColor || '#007bff');
        root.style.setProperty('--outline-button-text-color', this.themeSettings.outlineButtonTextColor || '#007bff');
        
        // Apply typography settings
        root.style.setProperty('--font-family', this.themeSettings.fontFamily || "'Inter', sans-serif");
        root.style.setProperty('--font-size', (this.themeSettings.fontSize || 16) + 'px');
        root.style.setProperty('--line-height', this.themeSettings.lineHeight || '1.6');
        root.style.setProperty('--letter-spacing', this.themeSettings.letterSpacing || '0.5px');
        
        // Apply visual settings
        root.style.setProperty('--border-radius', this.themeSettings.borderRadius || '8px');
        root.style.setProperty('--box-shadow', this.themeSettings.boxShadow || '0 2px 4px rgba(0,0,0,0.1)');
        
        // Apply animation settings
        root.style.setProperty('--transition-duration', this.themeSettings.transitionDuration || '0.3s');
        root.style.setProperty('--transition-timing', this.themeSettings.transitionTimingFunction || 'ease');
        
        // Apply page section colors
        root.style.setProperty('--header-background-color', this.themeSettings.headerBackgroundColor || '#ffffff');
        root.style.setProperty('--header-text-color', this.themeSettings.headerTextColor || '#333333');
        root.style.setProperty('--hero-background-color', this.themeSettings.heroBackgroundColor || '#f8f9fa');
        root.style.setProperty('--hero-text-color', this.themeSettings.heroTextColor || '#333333');
        root.style.setProperty('--about-background-color', this.themeSettings.aboutBackgroundColor || '#ffffff');
        root.style.setProperty('--about-text-color', this.themeSettings.aboutTextColor || '#333333');
        root.style.setProperty('--experience-background-color', this.themeSettings.experienceBackgroundColor || '#f8f9fa');
        root.style.setProperty('--experience-text-color', this.themeSettings.experienceTextColor || '#333333');
        root.style.setProperty('--projects-background-color', this.themeSettings.projectsBackgroundColor || '#ffffff');
        root.style.setProperty('--projects-text-color', this.themeSettings.projectsTextColor || '#333333');
        root.style.setProperty('--contact-background-color', this.themeSettings.contactBackgroundColor || '#f8f9fa');
        root.style.setProperty('--contact-text-color', this.themeSettings.contactTextColor || '#333333');
        root.style.setProperty('--footer-background-color', this.themeSettings.footerBackgroundColor || '#343a40');
        root.style.setProperty('--footer-text-color', this.themeSettings.footerTextColor || '#ffffff');
        
        // Apply hover effect settings
        root.style.setProperty('--link-hover-color', this.themeSettings.linkHoverColor || '#0056b3');
        root.style.setProperty('--button-hover-transform', this.themeSettings.buttonHoverTransform || 'translateY(-2px)');
        root.style.setProperty('--button-hover-shadow', this.themeSettings.buttonHoverShadow || '0 4px 8px rgba(0,0,0,0.2)');
        root.style.setProperty('--card-hover-transform', this.themeSettings.cardHoverTransform || 'translateY(-4px)');
        root.style.setProperty('--card-hover-shadow', this.themeSettings.cardHoverShadow || '0 8px 16px rgba(0,0,0,0.15)');
        root.style.setProperty('--image-hover-transform', this.themeSettings.imageHoverTransform || 'scale(1.05)');
        root.style.setProperty('--image-hover-shadow', this.themeSettings.imageHoverShadow || '0 6px 12px rgba(0,0,0,0.2)');
        
        // Apply page loader settings
        root.style.setProperty('--loader-color', this.themeSettings.loaderColor || '#007bff');
        root.style.setProperty('--loader-background-color', this.themeSettings.loaderBackgroundColor || '#ffffff');
        root.style.setProperty('--loader-size', this.themeSettings.loaderSize || '40px');
        root.style.setProperty('--loader-animation-duration', this.themeSettings.loaderAnimationDuration || '1s');
        root.style.setProperty('--loader-fade-out-duration', this.themeSettings.loaderFadeOutDuration || '0.5s');
        
        // Apply gradient if enabled
        if (this.themeSettings.useGradientBackground) {
            const gradient = `linear-gradient(${this.themeSettings.gradientDirection}, ${this.themeSettings.gradientStartColor}, ${this.themeSettings.gradientEndColor})`;
            root.style.setProperty('--background-gradient', gradient);
            root.style.setProperty('--background-color', 'transparent');
        }

        // Load Google Fonts if needed
        this.loadGoogleFonts(this.themeSettings.fontFamily);

        console.log('Theme applied successfully:', this.themeSettings);
    }

    applyDefaultTheme() {
        const root = document.documentElement;
        
        // Default modern theme
        root.style.setProperty('--primary-color', '#3b82f6');
        root.style.setProperty('--secondary-color', '#64748b');
        root.style.setProperty('--background-color', '#ffffff');
        root.style.setProperty('--text-color', '#1e293b');
        root.style.setProperty('--font-family', "'Inter', sans-serif");
        root.style.setProperty('--font-size', '16px');
        root.style.setProperty('--line-height', '1.6');
        root.style.setProperty('--letter-spacing', '0.5px');
        root.style.setProperty('--border-radius', '12px');
        root.style.setProperty('--box-shadow', '0 4px 8px rgba(0,0,0,0.15)');
        root.style.setProperty('--transition-duration', '0.3s');
        root.style.setProperty('--transition-timing', 'ease');
        
        // Default page section colors
        root.style.setProperty('--header-background-color', '#ffffff');
        root.style.setProperty('--header-text-color', '#1e293b');
        root.style.setProperty('--hero-background-color', '#f1f5f9');
        root.style.setProperty('--hero-text-color', '#1e293b');
        root.style.setProperty('--about-background-color', '#ffffff');
        root.style.setProperty('--about-text-color', '#1e293b');
        root.style.setProperty('--experience-background-color', '#f8fafc');
        root.style.setProperty('--experience-text-color', '#1e293b');
        root.style.setProperty('--projects-background-color', '#ffffff');
        root.style.setProperty('--projects-text-color', '#1e293b');
        root.style.setProperty('--contact-background-color', '#f1f5f9');
        root.style.setProperty('--contact-text-color', '#1e293b');
        root.style.setProperty('--footer-background-color', '#1e293b');
        root.style.setProperty('--footer-text-color', '#ffffff');
        
        // Default hover effects
        root.style.setProperty('--link-hover-color', '#1d4ed8');
        root.style.setProperty('--button-hover-transform', 'translateY(-2px)');
        root.style.setProperty('--button-hover-shadow', '0 4px 8px rgba(0,0,0,0.2)');
        root.style.setProperty('--card-hover-transform', 'translateY(-4px)');
        root.style.setProperty('--card-hover-shadow', '0 8px 16px rgba(0,0,0,0.15)');
        root.style.setProperty('--image-hover-transform', 'scale(1.05)');
        root.style.setProperty('--image-hover-shadow', '0 6px 12px rgba(0,0,0,0.2)');
        
        // Default page loader settings
        root.style.setProperty('--loader-color', '#3b82f6');
        root.style.setProperty('--loader-background-color', '#ffffff');
        root.style.setProperty('--loader-size', '40px');
        root.style.setProperty('--loader-animation-duration', '1s');
        root.style.setProperty('--loader-fade-out-duration', '0.5s');

        console.log('Default theme applied');
    }

    loadGoogleFonts(fontFamily) {
        if (!fontFamily || fontFamily.includes('Arial') || fontFamily.includes('Times')) {
            return; // Don't load for system fonts
        }

        const fontName = fontFamily.replace(/['"]/g, '').split(',')[0].trim();
        const link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = `https://fonts.googleapis.com/css2?family=${fontName}:wght@300;400;500;600;700&display=swap`;
        document.head.appendChild(link);
    }

    // Method to update theme dynamically (for admin panel)
    updateTheme(newSettings) {
        this.themeSettings = newSettings;
        this.applyTheme();
    }

    // Get current theme settings
    getCurrentTheme() {
        return this.themeSettings;
    }
}

// Initialize theme loader when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    window.themeLoader = new ThemeLoader();
});

// Export for use in other scripts
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ThemeLoader;
} 